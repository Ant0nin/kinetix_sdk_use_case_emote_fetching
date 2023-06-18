using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

// singleton
public sealed class KinetixRestApiClient
{
    public readonly string apiKey = "22f89ac029e3436d2d30aa34f3d544e7";

    private string _baseUri = "https://sdk-api.kinetix.tech/v1/emotes/";
    private HttpClient _httpClient;

    private static readonly Lazy<KinetixRestApiClient> lazy = new(() => new KinetixRestApiClient());
    public static KinetixRestApiClient Instance
    {
        get
        {
            return lazy.Value;
        }
    }


    private KinetixRestApiClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_baseUri)
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
    }

    public async Task<JObject> GetEmoteAsync(string uuid)
    {
        JObject emote = null;
        HttpResponseMessage response = await _httpClient.GetAsync(uuid);
        if(response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            emote = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(json);
        }
        return emote;
    }
}
