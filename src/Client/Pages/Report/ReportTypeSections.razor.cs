using Mapster;
using Microsoft.AspNetCore.Components;
using rmsweb.Client.Components.EntityTable;
using rmsweb.Client.Infrastructure.ApiClient;

namespace rmsweb.Client.Pages.Report;
public partial class ReportTypeSections
{
    [Parameter]
    public string Id { get; set; } = default!;
    [Inject]
    protected IReportTypeSectionsClient ReportTypeSectionsClient { get; set; } = default!;
    private EntityTable<ReportTypeSectionDto, Guid, ReportTypeSectionViewModel> _table = default!;
    protected EntityServerTableContext<ReportTypeSectionDto, Guid, ReportTypeSectionViewModel> Context { get; set; } = default!;

    private List<UserRoleDto> _userRolesList = default!;

    private string _title = string.Empty;
    private string _description = string.Empty;

    private string _searchString = string.Empty;

    private bool _canEditUsers;
    private bool _canSearchRoles;
    private bool _loaded;

    protected override void OnInitialized() =>
        Context = new(
            entityName: L["ReportTypeSection"],
            entityNamePlural: L["ReportTypeSections"],
            // entityResource: FSHResource.Products,
            fields: new()
            {
                new(prod => prod.Name, L["Name"], "Name"),
                new(prod => prod.Description, L["Description"], "Description"),
                new(prod => prod.ReportTypeId, L["ReportTypeId"], "ReportTypeId"),
            },
            enableAdvancedSearch: true,
            idFunc: reportType => reportType.Id,
            searchFunc: async filter =>
            {
                var reportTypeId = Guid.Parse(Id);
                var result = await ReportTypeSectionsClient.GetReportTypeSectionsByReportTypeAsync(reportTypeId);
                return result.Adapt<PaginationResponse<ReportTypeSectionDto>>();
            },
            createFunc: async prod =>
            {
                await ReportTypeSectionsClient.CreateReportTypeSectionAsync(prod.Adapt<CreateReportTypeSectionRequest>());
            },
            updateFunc: async (id, prod) =>
            {
                await ReportTypeSectionsClient.UpdateReportTypeSectionAsync(id, prod.Adapt<UpdateReportTypeSectionRequest>());
            });

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

    //private void ViewReport(in Guid reportTypeId) =>
    //   Navigation.NavigateTo($"/reportTypes/{reportTypeId}");

    //private void ManageReportSections(in Guid reportTypeId) =>
    //    Navigation.NavigateTo($"/reportTypes/{reportTypeId}/sections");
}

public class ReportTypeSectionViewModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid ReportTypeId { get; set; }
}

public class ReportTypeSectionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid ReportTypeId { get; set; }
}