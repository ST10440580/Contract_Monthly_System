using Contract_Monthly_System.Controllers;
using Contract_Monthly_System.Models;
using Contract_Monthly_System.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Contract_Monthly_System.Tests
{
    [TestClass]
    public class LecturerControllerTests
    {
        private LecturerController GetControllerWithSession(Mock<IDatabase> mockDb)
        {



            var controller = new LecturerController(mockDb.Object);

            var httpContext = new DefaultHttpContext();
            httpContext.Session = new MockHttpSession();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            return controller;
        }

        [TestMethod]
        public void SubmitClaim_InvalidLecturerName_ReturnsViewWithError()
        {
            // Arrange
            var mockDb = new Mock<IDatabase>();
            var controller = GetControllerWithSession(mockDb);
            var model = new LectureViewer
            {
                LecturerName = "", // Invalid
                Month = "2025-10",
                Module = "Web Dev",
                HoursWorked = 10,
                HourlyRate = 500
            };

            // Act
            var result = controller.SubmitClaim(model, null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsTrue(controller.ModelState.ContainsKey("LecturerName"));
        }
    }
}
