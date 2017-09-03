using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using Moq;

namespace Base64Diff.Api.Controllers
{
    using Domain;
    using Domain.Services;

    [TestFixture]
    public class DiffControllerTest
    {
        static Diff DiffStub = new Diff(new byte[] { 0 }, new byte[] { 1 });

        Mock<IDiffStore> StoreMock;
        DiffController Controller;

        /// <summary>
        /// Helper to assert a JsonResult response and return its string representation for further tests.
        /// </summary>
        string AssertJsonResponse(Func<IActionResult> action)
        {
            var result = action();
            Assert.IsInstanceOf<JsonResult>(result);
            var jsonResult = (JsonResult)result;
            return JsonConvert.SerializeObject(jsonResult.Value);
        }

        string AssertUnprocessableEntityResponse(Func<IActionResult> action)
        {
            var result = action();
            Assert.IsInstanceOf<ObjectResult>(result);
            var objResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status422UnprocessableEntity, objResult.StatusCode);
            return JsonConvert.SerializeObject(objResult.Value);
        }

        [SetUp]
        public void CreateSubject()
        {
            StoreMock = new Mock<IDiffStore>();
            Controller = new DiffController(StoreMock.Object);
        }

        [Test]
        public void TestGetDiffNotFound()
        {
            StoreMock.Setup(store => store.Get(1)).Returns((Diff)null);
            Assert.IsInstanceOf<NotFoundResult>(Controller.Get(1));
        }

        [Test]
        public void TestGetDiffFound()
        {
            StoreMock.Setup(store => store.Get(1)).Returns(DiffStub);
            var response = AssertJsonResponse(() => Controller.Get(1));
            Assert.AreEqual("{\"status\":\"different-content\",\"differences\":[{\"Offset\":0,\"Length\":1}]}", response);
        }

        [Test]
        public void TestSetLeftWithValidData()
        {
            StoreMock.Setup(store => store.SetLeft(1, "xyz")).Returns(DiffStub);
            var postData = new DiffController.DiffData { Data = "xyz" };
            var response = AssertJsonResponse(() => Controller.SetLeft(1, postData));
            Assert.AreEqual("{\"success\":true}", response);
        }

        [Test]
        public void TestSetLeftWithInvalidData()
        {
            StoreMock.Setup(store => store.SetLeft(1, "xyz")).Throws<FormatException>();
            var postData = new DiffController.DiffData { Data = "xyz" };
            var response = AssertUnprocessableEntityResponse(() => Controller.SetLeft(1, postData));
            Assert.AreEqual("{\"error\":\"Malformed Base64 string data\"}", response);
        }

        [Test]
        public void TestSetRight()
        {
            StoreMock.Setup(store => store.SetRight(1, "xyz")).Returns(DiffStub);
            var postData = new DiffController.DiffData { Data = "xyz" };
            var response = AssertJsonResponse(() => Controller.SetRight(1, postData));
            Assert.AreEqual("{\"success\":true}", response);
        }

        [Test]
        public void TestSetRightWithInvalidData()
        {
            StoreMock.Setup(store => store.SetRight(1, "xyz")).Throws<FormatException>();
            var postData = new DiffController.DiffData { Data = "xyz" };
            var response = AssertUnprocessableEntityResponse(() => Controller.SetRight(1, postData));
            Assert.AreEqual("{\"error\":\"Malformed Base64 string data\"}", response);
        }

        [Test]
        public void TestNullStoreConstructor()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new DiffController(null));
            Assert.AreEqual("diffStore", ex.ParamName);
        }
    }
}
