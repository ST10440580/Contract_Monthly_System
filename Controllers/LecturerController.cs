using System.Collections.Generic;
using System.Data.SqlClient;
using Contract_Monthly_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Contract_Monthly_System.Controllers
{
    public class LecturerController : Controller
    {
        [HttpGet]
        public IActionResult SubmitClaim()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitClaim(LectureViewer claim, IFormFile SupportingDocument)
        {
            string connectionString = @"Server=(localdb)\Contract_Monthly_System;Database=Contract_monthly_system;Trusted_Connection=True;";

            // 🔹 Validate LecturerName
            if (string.IsNullOrWhiteSpace(claim.LecturerName))
            {
                ModelState.AddModelError("LecturerName", "Lecturer name is required.");
                return View(claim);
            }

            int lecturerId;

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("SELECT LecturerID FROM Lecturers WHERE LecturerName = @LecturerName", connection))
                {
                    command.Parameters.AddWithValue("@LecturerName", claim.LecturerName);
                    connection.Open();
                    var result = command.ExecuteScalar();

                    if (result == null)
                    {
                        ModelState.AddModelError("LecturerName", "Lecturer not found.");
                        return View(claim);
                    }

                    lecturerId = Convert.ToInt32(result);
                }
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", "Database error occurred: " + ex.Message);
                return View(claim);
            }

            // 🔹 Handle file upload
            string filePath = null;

            if (SupportingDocument != null && SupportingDocument.Length > 0)
            {
                var extension = Path.GetExtension(SupportingDocument.FileName).ToLower();
                var allowed = new[] { ".pdf", ".docx", ".xlsx" };

                if (!allowed.Contains(extension))
                {
                    ModelState.AddModelError("SupportingDocument", "Only .pdf, .docx, and .xlsx files are allowed.");
                    return View(claim);
                }

                if (SupportingDocument.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("SupportingDocument", "File size must be under 5MB.");
                    return View(claim);
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + extension;
                filePath = Path.Combine("uploads", fileName);
                var fullPath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    SupportingDocument.CopyTo(stream);
                }
            }

            // 🔹 Finalize claim
            claim.SupportingDocumentPath = filePath;
            claim.LecturerID = lecturerId;

            // ✅ Store both ID and name in session
            HttpContext.Session.SetInt32("LecturerID", lecturerId);
            HttpContext.Session.SetString("LecturerName", claim.LecturerName);

            // ✅ Save claim and check result
            try
            {
                bool success = new Database().store_claim(claim);

                if (!success)
                {
                    ModelState.AddModelError("", "There was a problem saving your claim.");
                    return View(claim);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Exception during claim save: " + ex.Message);
                return View(claim);
            }

            TempData["Message"] = "Claim submitted successfully.";
            return RedirectToAction("TrackClaims");
        }


        [HttpGet]
        public IActionResult TrackClaims()
        {
            int? lecturerId = HttpContext.Session.GetInt32("LecturerID");

            if (lecturerId == null)
            {
                TempData["Message"] = "Session expired or lecturer not identified. Please log in again.";
                return RedirectToAction("Login", "Account");
            }
            var claims = new Database().GetClaimsByLecturerId(lecturerId.Value);

            // ✅ Safety check
            if (claims == null)
            {
                claims = new List<LectureViewer>();
            }

            ViewBag.userFullName = HttpContext.Session.GetString("LecturerName");

            return View(claims); // ✅ This must match the view's model
        }


    }
}
