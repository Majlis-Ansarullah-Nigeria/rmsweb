using FSH.WebApi.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using rmsweb.Client.Infrastructure.Common;
using Microsoft.AspNetCore.Components;
using rmsweb.Client.Components.EntityTable;
using rmsweb.Client.Infrastructure.ApiClient;

namespace rmsweb.Client.Pages.Report;

public partial class SubmissionWindows
{
    //[Parameter]
    //public string Id { get; set; } = default!;
    [Inject]
    protected ISubmissionWindowClient SubmissionWindowClient { get; set; } = default!;

    protected EntityServerTableContext<SubmissionWindowDto, Guid, SubmissionWindowViewModel> Context { get; set; } = default!;

    private EntityTable<SubmissionWindowDto, Guid, SubmissionWindowViewModel> _table = default!;


    protected override void OnInitialized() =>
        Context = new(
            entityName: L["SubmissionWindow"],
            entityNamePlural: L["SubmissionWindows"],
            // entityResource: FSHResource.Products,
            fields: new()
            {
                new(submissionwindow => submissionwindow.Name + " _ " + submissionwindow.Year + " _ " + submissionwindow.Month, L["Name"], "Name"),

                new(submissionwindow => submissionwindow.StartingDate + " _ " + submissionwindow.EndingDate, L["StartDate - EndDate"], "StartDate - EndDate"),

                new(submissionwindow => DateTime.Now >= submissionwindow.StartingDate && DateTime.Now <= submissionwindow.EndingDate ? "Current" : "Previous", L["Status"], "Status"),

                new(submissionwindow => submissionwindow.IsLocked, L["IsLocked"], "IsLocked"),
            },
            enableAdvancedSearch: true,
            idFunc: submissionWindow => submissionWindow.Id,
            searchFunc: async filter =>
            {
                var result = await SubmissionWindowClient.GetSubmissionWindowsAsync("1");
                return result.Adapt<PaginationResponse<SubmissionWindowDto>>();
                //var paginated = submissionWindowDto.Adapt<PaginationResponse<SubmissionWindowDto>>();
                //paginated.Data = submissionWindowDto;
                //return paginated;
            },
            createFunc: async prod =>
            {
                await SubmissionWindowClient.AddSubmissionWindowAsync("1", prod.Adapt<CreateSubmissionWindowRequest>());
            },
            updateFunc: async (id, prod) =>
            {
                await SubmissionWindowClient.UpdateSubmissionWindowAsync(id, "1", prod.Adapt<UpdateSubmissionWindowRequest>());
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

    private void ViewReport(in Guid reportTypeId) =>
       Navigation.NavigateTo($"/reportTypes/{reportTypeId}");

    private void ManageReportSections(in Guid reportTypeId) =>
        Navigation.NavigateTo($"/reportTypes/{reportTypeId}/sections");
}

public class SubmissionWindowViewModel
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

public class SubmissionWindowDto
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
