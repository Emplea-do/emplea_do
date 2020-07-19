using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Util;
using LegacyAPI;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using Newtonsoft.Json;

public class LegacyJobCardsResult
{
    public List<JobCardDTO> Jobs;
    public int JobCount;
    public int PagesCount;
    public int CurrentPage;
    public int PageSize;
}
public class LegacyApiClient
{
     private readonly IFeatureManager _featureManager;
    private readonly IConfiguration _configuration;

    public LegacyApiClient(IFeatureManager featureManager, IConfiguration configuration)
    {
        _featureManager = featureManager;
        _configuration = configuration;
    }
    
    public async Task<IList<JobCardDTO>> GetJobsFromLegacy()
    {
        //if(_featureManager.IsEnabled(FeatureFlags.LegacyClient.UseMockData))
           return GetJobsFromMockData();
        //return await GetJobsFromLegacyCore();
        
    }

    public async Task<JobCardDTO> GetJobById(string Id)
    {
        if(await _featureManager.IsEnabledAsync(FeatureFlags.LegacyClient.UseMockData))
        return  GetJobByIdFromMockData(Id);

        return await GetJobByIdCore(Id);
    }

    private async Task<IList<JobCardDTO>> GetJobsFromLegacyCore()
    {
        int pageSize = _configuration.GetValue<int>(ConfigurationFlags.LegacyApiClient.PageSize, 10);
        
        var r = await GetBaseAPIUrl()
                    .AppendPathSegment("jobs")
                    .SetQueryParams(new { pagesize = pageSize, page = 1 })  // This should be parameterized in the future. 
                    .GetJsonAsync<LegacyJobCardsResult>();

        return r.Jobs;
    }

    private string GetBaseAPIUrl()
    {
        return _configuration.GetValue("BaseAPIUrl", string.Empty);
    }

    private async Task<JobCardDTO> GetJobByIdCore(string Id)
    {

        var jobs = await GetJobsFromLegacy();
        var j = jobs.FirstOrDefault(i => i.Link == Id);

        return j;

        }

    private IList<JobCardDTO> GetJobsFromMockData()
    {
        var r = JsonConvert.DeserializeObject<LegacyJobCardsResult>(FakeData.GetFakeData());
        return r.Jobs;
    }

    private JobCardDTO GetJobByIdFromMockData(string Id)
    {
        var list = GetJobsFromMockData();
        return list.FirstOrDefault(j=> j.Link == Id);
    }

}


public static class FakeData

{
    public static string GetFakeData()
    {
        return
        @"{
Jobs: [
{
Link: '1298',
CompanyName: 'Scopic Software',
Title: 'Remote HTML/CSS and WordPress Developer',
JobType: 'Tiempo Completo',
Location: 'N/A',
PublishedDate: '2019-11-08T12:40:12.987',
IsRemote: true,
ViewCount: 24,
Likes: 0,
CompanyLogoUrl: null,
Description: '--',
HowToApply: '--',
Email:'test@mail'
},
{
Link: '1297',
CompanyName: 'Ntech',
Title: 'QUALITY ASSURANCE',
JobType: 'Tiempo Completo',
Location: 'N/A',
PublishedDate: '2019-11-07T13:44:47.7',
IsRemote: false,
ViewCount: 40,
Likes: 0,
CompanyLogoUrl: null,
Description: '---',
HowToApply: '---',
Email:'test@mail'
},
{
Link: '1296',
CompanyName: 'HELPNET',
Title: 'SENIOR iOS DEVELOPER',
JobType: 'Tiempo Completo',
Location: 'N/A',
PublishedDate: '2019-11-06T20:12:34.113',
IsRemote: false,
ViewCount: 47,
Likes: 0,
CompanyLogoUrl: 'http://www.help-net.net/logo.png',
Description: '<p> We’re a USA based company in Miami, FL, and Santiago DR, and we’re looking for candidates like you to fill an IOS Developer position that focuses on designing/implementing the overall architecture of mobile applications that interacts with complex API’s and implement complex business logic and an awesome UI/UX. We’d love to hear from qualified, self-learner and proactive people that loves creating wonderful and scalable apps with high quality and standardized code. <br></p><p> Expertise in:<br>- Apple’s Xcode IDE<br>- Swift (3.0 and above).<br>- Both iPhone and iPad Apps and Architectures<br>- Apple Frameworks and APIs<br><br>Vast Knowledge of:<br><br>- Mobile development and best practices<br>- Application life cycle including Certificates, Profiles and Publishing to the App Store<br>- Strong understanding of security best practices at the application and network level<br>- UI and UX design experience<br>- Consuming REST APIs and JSON data.<br>- Apple Human Interface Guidelines<br>- Implementation of Push Notifications<br>- Git code repository technology<br><br>Ideal / Optional / Plus:<br><br>- TFS<br>- Jira<br>- Scrum and Agile methodologies <br></p>',
HowToApply: '<p>Please send your resume to hr@help-net.net and answer the following questions:</p><p>What is your English level?<br>How many years working with native iOS development do you have?<br></p>'
,
Email:'test@mail'},
{
Link: '1295',
CompanyName: 'NtechSRL',
Title: 'Java Dev',
JobType: 'Tiempo Completo',
Location: 'N/A',
PublishedDate: '2019-11-06T16:30:10.493',
IsRemote: false,
ViewCount: 51,
Likes: 0,
CompanyLogoUrl: null,
Description: '---',
HowToApply: '---',
Email:'test@mail'
},
]
}";
}
} 

