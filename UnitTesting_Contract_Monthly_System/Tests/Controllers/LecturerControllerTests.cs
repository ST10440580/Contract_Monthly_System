using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Contract_Monthly_System.Controllers;
using Contract_Monthly_System.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class LecturerControllerTests
{
    private LecturerController GetControllerWithSession()
    {
        var controller = new LecturerController();

        var httpContext = new DefaultHttpContext();
        httpContext.Session = new MockHttpSession();
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        return controller;
    }

    [Fact]
    public void SubmitClaim_InvalidLecturerName_ReturnsViewWithError()
    {
        var controller = GetControllerWithSession();
        var model = new LectureViewer { LecturerName = "" };

        var result = controller.SubmitClaim(model, null);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(controller.ModelState.IsValid);
        Assert.True(controller.ModelState.ContainsKey("LecturerName"));
    }

    [Fact]
    public void SubmitClaim_ValidClaimWithoutFile_RedirectsToTrackClaims()
    {
        var controller = GetControllerWithSession();
        var model = new LectureViewer
        {
            LecturerName = "Tshepang",
            Month = "2025-10",
            Module = "Web Dev",
            HoursWorked = 10,
            HourlyRate = 500
        };

        controller.HttpContext.Session.SetInt32("LecturerID", 1);

        var result = controller.SubmitClaim(model, null);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("TrackClaims", redirect.ActionName);
    }

    [Fact]
    public void SubmitClaim_FileTooLarge_ReturnsViewWithError()
    {
        var controller = GetControllerWithSession();
        var model = new LectureViewer
        {
            LecturerName = "Tshepang",
            Month = "2025-10",
            Module = "Web Dev",
            HoursWorked = 10,
            HourlyRate = 500
        };

        var fileMock = new FormFile(new MemoryStream(new byte[6 * 1024 * 1024]), 0, 6 * 1024 * 1024, "Data", "test.pdf");

        var result = controller.SubmitClaim(model, fileMock);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.True(controller.ModelState.ContainsKey("SupportingDocumentPath"));
    }

    [Fact]
    public void TrackClaims_NoSession_RedirectsToLogin()
    {
        var controller = new LecturerController();
        var result = controller.TrackClaims();

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", redirect.ActionName);
    }

    [Fact]
    public void TrackClaims_WithSession_ReturnsClaimsView()
    {
        var controller = GetControllerWithSession();
        controller.HttpContext.Session.SetInt32("LecturerID", 1);
        controller.HttpContext.Session.SetString("LecturerName", "Tshepang");

        var result = controller.TrackClaims();

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsType<List<LectureViewer>>(viewResult.Model);
    }
}
