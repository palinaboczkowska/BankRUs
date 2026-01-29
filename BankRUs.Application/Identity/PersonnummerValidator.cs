using System.Net.Http.Json;

namespace BankRUs.Application.Identity
{
    public class TestPersonnummerValidator
    {
        private readonly HttpClient _http;

        public TestPersonnummerValidator(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> IsValidAsync(string ssn)
        {
            var url = "https://skatteverket.entryscape.net/rowstore/dataset/b4de7df7-63c0-4e7e-bb59-1f156a591763/json";

            var response = await _http.GetFromJsonAsync<TestPersonnummerResponse>(url);

            return response?.Results?.Any(r =>
                r.TryGetValue("testpersonnummer", out var value) &&
                value == ssn
            ) == true;
        }

        private class TestPersonnummerResponse
        {
            public List<Dictionary<string, string>>? Results { get; set; }
        }
    }


}
