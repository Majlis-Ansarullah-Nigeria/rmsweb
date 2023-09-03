using Mapster;
using Microsoft.AspNetCore.Components;
using rmsweb.Client.Components.EntityTable;
using rmsweb.Client.Infrastructure.ApiClient;

namespace rmsweb.Client.Pages.ReportSubmission;
public partial class ReportSubmission
{
    [Inject]
    protected ISubmissionWindowClient SubmissionWindowClient { get; set; } = default!;

    protected EntityServerTableContext<ReportSubmissionDto, Guid, ReportSubmissionViewModel> Context { get; set; } = default!;
    private bool _canViewRoles;

    private EntityTable<ReportSubmissionDto, Guid, ReportSubmissionViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: L["ReportSubmission"],
            entityNamePlural: L["ReportSubmissions"],
            // entityResource: FSHResource.Products,
            fields: new()
            {
                new(prod => prod.Name, L["Name"], "Name"),
            },
            enableAdvancedSearch: true,
            idFunc: reportType => reportType.Id,
            searchFunc: async filter =>
            {
                var result = await SubmissionWindowClient.GetSubmissionWindowsAsync("1");
                return result.Adapt<PaginationResponse<ReportSubmissionDto>>();
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
       Navigation.NavigateTo($"/reportSubmissions/{submissionId}");
}

public class ReportSubmissionViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}

public class ReportSubmissionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}