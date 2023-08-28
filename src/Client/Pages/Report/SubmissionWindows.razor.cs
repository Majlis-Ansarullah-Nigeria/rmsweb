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
                new(submissionwindow => submissionwindow.SubmissionWindowDataDto.Name + " _ " + submissionwindow.SubmissionWindowDataDto.Year + " _ " + submissionwindow.SubmissionWindowDataDto.Month, L["Name"], "Name"),

                new(submissionwindow => submissionwindow.SubmissionWindowDataDateDto.StartDate + " _ " + submissionwindow.SubmissionWindowDataDateDto.EndDate, L["StartDate - EndDate"], "StartDate - EndDate"),

                new(submissionwindow => submissionwindow.IsLocked, L["IsLocked"], "IsLocked"),
            },
            enableAdvancedSearch: true,
            idFunc: reportType => reportType.SubmissionWindowId,
            searchFunc: async filter =>
            {
                //var result = await SubmissionWindowClient.GetSubmissionWindowAsync("1", filter);
                //return result.Adapt<PaginationResponse<SubmissionWindowDto>>();
                var submissionWindowDto = new List<SubmissionWindowDto>
                {
                    new SubmissionWindowDto(),
                    new SubmissionWindowDto(),
                    new SubmissionWindowDto(),
                };
                var paginated = submissionWindowDto.Adapt<PaginationResponse<SubmissionWindowDto>>();
                paginated.Data = submissionWindowDto;
                return paginated;
            },
            createFunc: async prod =>
            {
                await SubmissionWindowClient.AddSubmissionWindowAsync("1", prod.Adapt<CreateSubmissionWindowRequest>());
            },
            updateFunc: async (id, prod) =>
            {
                await SubmissionWindowClient.UpdateSubmissionWindowAsync(id, "1", prod.Adapt<UpdateSubmissionWindowRequest>());
            });
    //exportFunc: async filter =>
    //{
    //    var exportFilter = filter.Adapt<ExportProductsRequest>();

    //    exportFilter.BrandId = SearchBrandId == default ? null : SearchBrandId;
    //    exportFilter.MinimumRate = SearchMinimumRate;
    //    exportFilter.MaximumRate = SearchMaximumRate;

    //    return await ProductsClient.ExportAsync(exportFilter);
    //},
    //deleteFunc: async id => await ProductsClient.DeleteAsync(id));

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
    public SubmissionWindowDataDto SubmissionWindowDataDto { get; set; } = new SubmissionWindowDataDto
    {
        Name = "Majlis",
        Month = 10,
        Year = 2020,
    };
    public SubmissionWindowDataDateDto SubmissionWindowDataDateDto { get; set; } = new SubmissionWindowDataDateDto
    {
        StartDate = DateTime.Now,
        EndDate = DateTime.Now.AddDays(2),
    };
    public Guid SubmissionWindowId { get; set; }
    public Guid ReportTypeId { get; set; }
    public Guid ReportTypeName { get; set; }
    public bool IsLocked { get; set; }
}

public class SubmissionWindowDto
{
    public SubmissionWindowDataDto SubmissionWindowDataDto { get; set; } = new SubmissionWindowDataDto
    {
        Name = "Majlis",
        Month = 10,
        Year = 2020,
    };
    public SubmissionWindowDataDateDto SubmissionWindowDataDateDto { get; set; } = new SubmissionWindowDataDateDto
    {
        StartDate = DateTime.Now,
        EndDate = DateTime.Now.AddDays(2),
    };
    public Guid SubmissionWindowId { get; set; }
    public Guid ReportTypeId { get; set; }
    public Guid ReportTypeName { get; set; }
    public bool IsLocked { get; set; }
}

public class SubmissionWindowDataDto
{
    public string Name { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}

public class SubmissionWindowDataDateDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}