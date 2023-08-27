using FSH.WebApi.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using rmsweb.Client.Components.EntityTable;
using rmsweb.Client.Infrastructure.ApiClient;
using rmsweb.Client.Infrastructure.Common;

namespace rmsweb.Client.Pages.Report;
public partial class ReportTypes
{
    [Inject]
    protected IReportTypesClient ReportTypesClient { get; set; } = default!;

    protected EntityServerTableContext<ReportTypeDto, Guid, ReportTypeViewModel> Context { get; set; } = default!;
    private bool _canViewRoles;

    private EntityTable<ReportTypeDto, Guid, ReportTypeViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: L["ReportType"],
            entityNamePlural: L["ReportTypes"],
           // entityResource: FSHResource.Products,
            fields: new()
            {
                new(prod => prod.Name, L["Name"], "Name"),
                new(prod => prod.Description, L["Description"], "Description"),
                new(prod => prod.ReportTag, L["Report Tag"], "Report Tag"),
            },
            enableAdvancedSearch: true,
            idFunc: reportType => reportType.Id,
            searchFunc: async filter =>
            {
                var result = await ReportTypesClient.GetReportTypesAsync(filter);
                return result.Adapt<PaginationResponse<ReportTypeDto>>();
            },
            createFunc: async prod =>
            {
                await ReportTypesClient.CreateReportTypeAsync(prod.Adapt<CreateReportTypeRequest>());
            },
            updateFunc: async (id, prod) =>
            {
                await ReportTypesClient.UpdateReportTypeAsync(id, prod.Adapt<UpdateReportTypeRequest>());
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

public class ReportTypeViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ReportTag { get; set; } = "Test";
}

public class ReportTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ReportTag { get; set; } = "Test";
}