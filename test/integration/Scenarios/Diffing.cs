using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Text;

namespace Base64Diff.IntegrationTests.Scenarios
{
    [TestFixture]
    public class Diffing : IntegrationTest
    {
        // The quick brown fox jumps over the lazy dog
        static readonly HttpContent TheQuickBrownFoxJumpsOverTheLazyDog = new StringContent(
            "{\"data\":\"VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZw==\"}", Encoding.UTF8, "application/json");

        // The quick brown fox jumps over the lazy cat
        static readonly HttpContent TheQuickBrownFoxJumpsverTheLazyCat = new StringContent(
            "{\"data\":\"VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGNhdA==\"}", Encoding.UTF8, "application/json");

        // Foo bar
        static readonly HttpContent FooBar = new StringContent(
            "{\"data\":\"Rm9vIGJhcg==\"}", Encoding.UTF8, "application/json");

        // Invalid
        static readonly HttpContent InvalidContent = new StringContent(
            "{\"data\":\"not_a_base64_string\"}", Encoding.UTF8, "application/json");

        [Test]
        public async Task client_gets_404_for_nonexisting_diff()
        {
            var response = await Client.GetAsync($"/v1/diff/{int.MinValue}");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task client_sets_diff_left_part()
        {
            var response = await Client.PutAsync("/v1/diff/1/left", TheQuickBrownFoxJumpsOverTheLazyDog);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("{\"success\":true}", body);
            response = await Client.GetAsync("/v1/diff/1");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("{\"status\":\"different-lengths\",\"differences\":[]}", body);
        }

        [Test]
        public async Task client_sets_diff_right_part()
        {
            var response = await Client.PutAsync("/v1/diff/2/right", TheQuickBrownFoxJumpsOverTheLazyDog);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("{\"success\":true}", body);
            response = await Client.GetAsync("/v1/diff/2");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("{\"status\":\"different-lengths\",\"differences\":[]}", body);
        }

        [Test]
        public async Task client_sets_both_parts_of_diff_with_different_lenghts()
        {
            var response = await Client.PutAsync("/v1/diff/3/left", TheQuickBrownFoxJumpsOverTheLazyDog);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("{\"success\":true}", body);
            response = await Client.PutAsync("/v1/diff/3/right", FooBar);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("{\"success\":true}", body);
            response = await Client.GetAsync("/v1/diff/3");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("{\"status\":\"different-lengths\",\"differences\":[]}", body);
        }

        [Test]
        public async Task client_sets_both_parts_of_diff_with_same_content()
        {
            var response = await Client.PutAsync("/v1/diff/4/left", FooBar);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("{\"success\":true}", body);
            response = await Client.PutAsync("/v1/diff/4/right", FooBar);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("{\"success\":true}", body);
            response = await Client.GetAsync("/v1/diff/4");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("{\"status\":\"same-content\",\"differences\":[]}", body);
        }

        [Test]
        public async Task client_sets_both_parts_of_diff_with_different_content()
        {
            var response = await Client.PutAsync("/v1/diff/5/left", TheQuickBrownFoxJumpsOverTheLazyDog);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("{\"success\":true}", body);
            response = await Client.PutAsync("/v1/diff/5/right", TheQuickBrownFoxJumpsverTheLazyCat);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("{\"success\":true}", body);
            response = await Client.GetAsync("/v1/diff/5");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("{\"status\":\"different-content\",\"differences\":[{\"offset\":40,\"length\":3}]}", body);
        }

        [Test]
        public async Task client_sends_invalid_base64_data_to_setleft_endpoint()
        {
            var response = await Client.PutAsync("/v1/diff/6/left", InvalidContent);
            Assert.AreEqual(422, (int)response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("{\"error\":\"Malformed Base64 string data\"}", body);
        }

        [Test]
        public async Task client_sends_invalid_base64_data_to_setright_endpoint()
        {
            var response = await Client.PutAsync("/v1/diff/7/right", InvalidContent);
            Assert.AreEqual(422, (int)response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("{\"error\":\"Malformed Base64 string data\"}", body);
        }
    }
}
