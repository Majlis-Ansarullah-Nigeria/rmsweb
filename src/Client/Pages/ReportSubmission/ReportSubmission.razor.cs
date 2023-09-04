using Mapster;
using Microsoft.AspNetCore.Components;
using rmsweb.Client.Components.EntityTable;
using rmsweb.Client.Infrastructure.ApiClient;
using rmsweb.Client.Pages.Report;

namespace rmsweb.Client.Pages.ReportSubmission;
public partial class ReportSubmission
{
    [Inject]
    protected ISubmissionWindowClient SubmissionWindowClient { get; set; } = default!;

    protected EntityServerTableContext<SubmissionWindowDto, Guid, SubmissionWindowViewModel> Context { get; set; } = default!;
    private bool _canViewRoles;

    private EntityTable<SubmissionWindowDto, Guid, SubmissionWindowViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: L["ReportSubmission"],
            entityNamePlural: L["ReportSubmissions"],
            // entityResource: FSHResource.Products,
            fields: new()
            {
                 new(submissionwindow => submissionwindow.Name + " _ " + submissionwindow.Year + " _ " + submissionwindow.Month, L["Name"], "Name"),

                 new(submissionwindow => submissionwindow.StartingDate + " _ " + submissionwindow.EndingDate, L["StartDate - EndDate"], "StartDate - EndDate")
            },
            enableAdvancedSearch: true,
            idFunc: submissionWindow => submissionWindow.Id,
            searchFunc: async filter =>
            {
                var result = await SubmissionWindowClient.GetSubmissionWindowsAsync("1");
                var result1 = GenerateMockData();
                return result.Adapt<PaginationResponse<SubmissionWindowDto>>();
            });

    // Advanced Search

    private Guid _searchBrandId;
    private Guid SearchBrandId
    {
        get => _searchBrandId;
        set
        {
            _searchBrandId = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private decimal _searchMinimumRate;
    private decimal SearchMinimumRate
    {
        get => _searchMinimumRate;
        set
        {
            _searchMinimumRate = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private decimal _searchMaximumRate = 9999;
    private decimal SearchMaximumRate
    {
        get => _searchMaximumRate;
        set
        {
            _searchMaximumRate = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private void MakeSubmission(in Guid submissionId) =>
       Navigation.NavigateTo($"/reportSubmission/{submissionId}");


    public List<ReportSubmissionDto> GenerateMockData()
    {
        return new List<ReportSubmissionDto>
        {
            new ReportSubmissionDto
            {
                Id = Guid.NewGuid(),
                Name = "Submission 1",
                Month = 1,
                Year = 2023,
                ReportTypeId = Guid.NewGuid(),
                IsLocked = false,
                StartingDate = DateTime.Now.AddDays(1),
                 EndingDate = DateTime.Now.AddDays(31)
            },
            new ReportSubmissionDto
            {
                Id = Guid.NewGuid(),
                Name = "Submission 2",
                Month = 2,
                Year = 2023,
                ReportTypeId = Guid.NewGuid(),
                IsLocked = true,
                StartingDate = DateTime.Now.AddDays(32),
                EndingDate = DateTime.Now.AddDays(62)
            }
        };
    }
}

public class ReportSubmissionViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public Guid ReportTypeId { get; set; }
    public bool IsLocked { get; set; }
    public DateTime StartingDate { get; set; }
    public DateTime EndingDate { get; set; }
}

public class ReportSubmissionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public Guid ReportTypeId { get; set; }
    public bool IsLocked { get; set; }
    public DateTime StartingDate { get; set; }
    public DateTime EndingDate { get; set; }
}