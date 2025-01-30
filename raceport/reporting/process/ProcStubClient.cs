using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace reporting.process
{
    public static class ProcStubClient
    {
        private static string _baseAddress = "http://sentry.verityclouds.com";

        public static async Task<ProcStubDto> createStub(string title) {
            var stub = new ProcStubDto { Name = title};

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsJsonAsync("api/procs", stub);

                if (response.IsSuccessStatusCode)
                {
                    stub = await response.Content.ReadAsAsync<ProcStubDto>();
                }
            }

            return stub;
        }

        public static async Task<ProcStubDto> updateStub(ProcStubDto _param)
        {
            var stub = new ProcStubDto ();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsJsonAsync("api/procs/tick", _param);

                if (response.IsSuccessStatusCode)
                {
                    stub = await response.Content.ReadAsAsync<ProcStubDto>();
                }
            }

            return stub;
        }
    }
}
