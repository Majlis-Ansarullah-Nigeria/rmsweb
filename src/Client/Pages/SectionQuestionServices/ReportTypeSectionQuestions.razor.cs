using FSH.WebApi.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using rmsweb.Client.Components.EntityTable;
using rmsweb.Client.Infrastructure.ApiClient;
using rmsweb.Client.Infrastructure.Auth;
using rmsweb.Client.Shared;
using System.Runtime.CompilerServices;

namespace rmsweb.Client.Pages.SectionQuestionServices;

public partial class ReportTypeSectionQuestions
{

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;
    [Parameter]
    public Guid ReportTypeSectionId { get; set; }

    [Inject]
    protected ISectionQuestionServicesClient SectionQuestionServicesClient { get; set; } = default!;

    protected EntityServerTableContext<QuestionDto, Guid, ReportQuestionRequest> Context { get; set; } = default!;

    private string _title = string.Empty;
    private string _description = string.Empty;

    private string _searchString = string.Empty;

    private bool _canEditUsers;
    private bool _canSearchRoles;
    private bool _loaded;
    private bool _canExportUsers;
    private bool _canViewRoles;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canExportUsers = await AuthService.HasPermissionAsync(user, FSHAction.Export, FSHResource.Users);
        _canViewRoles = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.UserRoles);

        Context = new(
         entityName: L["Question"],
            entityNamePlural: L["Questions"],
            entityResource: FSHResource.Questions,
            fields: new()
            {
                new(question => question.Text, L["Detail"]),
                new(question => question.Points, L["Point"])

            },
            hasExtraActionsFunc: () => true,
            canUpdateEntityFunc: e => false,
            createAction: string.Empty,
            canDeleteEntityFunc: e => false,
            searchFunc: async filter =>
            {
                var returnObject = new PaginationResponse<ReportQuestionsModel>();
                var result = await SectionQuestionServicesClient.GetSectionQuestionsBySectionIdAsync(ReportTypeSectionId, "1");
                return result.Adapt<PaginationResponse<QuestionDto>>();
            },
            exportAction: string.Empty);

    }

    private void ViewProfile(in Guid questionId) =>
       Navigation.NavigateTo($"/sectionquestionservices/{questionId}/EditQuestion");

    private void ManageRoles(in Guid userId) =>
       Navigation.NavigateTo($"/users/{userId}/roles");

}









