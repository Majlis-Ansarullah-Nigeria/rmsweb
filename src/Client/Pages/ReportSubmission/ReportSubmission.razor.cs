//using FSH.WebApi.Shared.Authorization;
//using FSH.WebApi.Shared.Multitenancy;
//using Mapster;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Authorization;
//using MudBlazor;
//using rmsweb.Client.Components.EntityTable;
//using rmsweb.Client.Infrastructure.ApiClient;
//using rmsweb.Client.Infrastructure.Auth;
//using rmsweb.Client.Shared;
//using System.Security.Claims;

//namespace rmsweb.Client.Pages.ReportSubmission;
//public partial class ReportSubmission
//{
//    private ReportQuestionsModel Report { get; set; }
//    [Inject]
//    protected ISectionQuestionServicesClient Client { get; set; } = default!;


//    public string _title = string.Empty;
//    public string _description = string.Empty;

//    private string _searchString = string.Empty;
//    private bool _loaded;
//    protected EntityServerTableContext<PrintedCardRequestResponseModel, Guid, GetAllPrintedCardRequests> Context { get; set; } = default!;

//    private EntityTable<PrintedCardRequestResponseModel, Guid, GetAllPrintedCardRequests> _table = default!;

//    protected override async Task OnInitializedAsync()
//    {
//        Context = new(
//           entityName: L["Card"],
//           entityNamePlural: L["PrintedCardsExports"],
//           entityResource: "Report Types",
//           fields: new()
//           {
//                new(prod => prod.Name, L["Name"], "Name"),
//                new(prod => prod.Description, L["Description"], "Description")
//           },
//           enableAdvancedSearch: true,
//           hasExtraActionsFunc: () => false,
//           idFunc: prod => Guid.Parse(prod.ChandaNo),
//           canUpdateEntityFunc: e => false,
//           searchFunc: async (filter) =>
//           {
//              /* var cardRequestFilter = filter.Adapt<GetAllPrintedCardRequests>();
//               var result = await CardRequestsClient.GetPrintedCardRequestAsync(cardRequestFilter);*/
//               return new();
//           }
//          /* deleteFunc: async id => await DilasClient.DeleteAsync(id)*/);

//        // Advanced Search
//        // await GenerateMockDataAsync();
//        _loaded = true;
//    }

//    private async Task GenerateMockDataAsync()
//    {
//        Guid type = new Guid("2d0d5733-5b43-485c-daee-08db7e658745");
//        var report = await Client.GetReportTypeQuestionsAsync(type, "1");
//        Report = report.Data;

//    }



//    public enum QuestionInputType
//    {
//        TextInput,
//        Checkbox,
//        Integer,
//        Dropdown,
//        Radio,
//        Date,
//        File
//    }

//    private void SaveSection(Sections section)
//    {
//        // Perform save operation for the specific section
//        // You can access the questions and answers using section.Questions
//        // Example: section.Questions[0].Answer
//    }

//    private void SaveReport()
//    {
//        // Perform save operation for the entire report
//        // You can access the sections, questions, and answers using Report.Sections
//        // Example: Report.Sections[0].Questions[0].Answer
//    }

//    private bool IsQuestionAnswered(ReportSectionQuestion question)
//    {
//        switch (question.ResponseType)
//        {
//            case ResponseType.Checkbox:
//                return true; // Checkbox type always considered answered
//            case ResponseType.Integer:
//            //  return question.AnswerInteger.HasValue;
//            default:
//                return !string.IsNullOrWhiteSpace(question.Text);
//                // return !string.IsNullOrWhiteSpace(question.Answer);
//        }
//    }

//    private void HandleFileUpload(/*IMudFileUploadEntry[] files*/)
//    {
//        // Handle file upload logic here
//    }
//    private Color GetGroupBadgeColor(int selected, int all)
//    {
//        if (selected == 0)
//            return Color.Error;

//        if (selected == all)
//            return Color.Success;

//        return Color.Info;
//    }

//    private bool Search(PermissionViewModel permission) =>
//        string.IsNullOrWhiteSpace(_searchString)
//            || permission.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) is true
//            || permission.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase) is true;
//}

//public record PermissionViewModel : FSHPermission
//{
//    public bool Enabled { get; set; }

//    public PermissionViewModel(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
//        : base(Description, Action, Resource, IsBasic, IsRoot)
//    {
//    }
//}