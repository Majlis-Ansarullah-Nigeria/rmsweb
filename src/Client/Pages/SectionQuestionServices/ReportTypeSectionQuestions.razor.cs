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
    public string ReportTypeSectionId { get; set; } = default!;

    [Inject]
    protected ISectionQuestionClient SectionQuestionClient { get; set; } = default!;

    protected EntityServerTableContext<QuestionDto, Guid, ReportQuestionRequest> Context { get; set; } = default!;

   
    private bool _loaded;
    //private bool _canExportUsers;
    //private bool _canViewRoles;

    protected override async Task OnInitializedAsync()
    {
        //var user = (await AuthState).User;
        //_canExportUsers = await AuthService.HasPermissionAsync(user, FSHAction.Export, FSHResource.Users);
        //_canViewRoles = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.UserRoles);

        Context = new(
         entityName: L["Question"],
            entityNamePlural: L["Questions"],
            //entityResource: FSHResource.Questions,
            fields: new()
            {
                new(question => question.Text, L["Detail"]),
                new(question => question.Points, L["Point"])

            },
            hasExtraActionsFunc: () => true,
            searchFunc: async filter =>
            {
               // var returnObject = new PaginationResponse<ReportQuestionsModel>();
                var result = await SectionQuestionClient.GetSectionQuestionsBySectionIdAsync(Guid.Parse(ReportTypeSectionId), "1");
                return result.Adapt<PaginationResponse<QuestionDto>>();
            },
             createFunc: async prod =>
             {
                 var SectionId = Guid.Parse(ReportTypeSectionId);
                 prod.SectionId = SectionId;
                 await SectionQuestionClient.AddQuestionAsync("1", prod.Adapt<ReportQuestionRequest>());
             },
            exportAction: string.Empty);

    }

    private void EditQuestion(in Guid questionId) =>
       Navigation.NavigateTo($"/sectionquestionservices/{questionId}/EditQuestion");

    public enum QuestionInputType
    {
        TextInput,
        Checkbox,
        Integer,
        Dropdown,
        Radio,
        Date,
        File
    }

}









