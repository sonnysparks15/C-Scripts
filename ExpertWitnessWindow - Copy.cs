// ///////////////////////////////////////////////////////////////////////

// ExpertWitnessWindow.cs
// Calls from StartMenuFactory.cs --> ExpertWitnessAdmin Method
// Sonny Sparks

// ///////////////////////////////////////////////////////////////////////
using System;
using System.DHTML;
using Ext;
using Ext.data;
using Ext.form;
using Ext.grid;
using Ext.menu;
using Ext.util;
using Ext.ux.grid;
using FireSharp;
using Versentia.Web.Scripts.Services;
using Versentia.Web.Scripts.UI;
using Versentia.Web.Scripts.Utils;

namespace Versentia.Web.Scripts.Accounts
{
    public class ExpertWitnessWindow : Observable
    {
        private GroupedWindow window;
        private readonly Dictionary loadOptions = new Dictionary("params", new Dictionary("start", 0, "limit", 50));
        private GridPanel expertWitnessGrid;
        private GridPanel expertWitnessGrid2;
        private JsonStore expertWitnessStore;
        private Label error;
        private string windowID;
        private DelayedTask searchTask;
        private Button btnClear;
        private Button btnAddConflictCheck;
        private Button btnRemoveConflictCheck;
        private DelayedTask eraseError;
        private double selectedIndex;
        private LoadMask loadMask;
        private Panel customFieldExpandedPanel;
        private Menu contextMenu;
        private Dictionary displayValues;

        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Creates Overall Window
        //Called from StartMenuFactory.cs
        public void ShowWindow(Element openFrom)
        {
            windowID = "ExpertWitnessAdmin";
            if (GroupedWindow.BringExistingToFront(windowID)) return;
            eraseError = new DelayedTask(new Callback(ClearError));
            searchTask = new DelayedTask(new Callback(delegate { expertWitnessStore.load(loadOptions); }));
            CreateGrid();
            CreateWindow();
            window.show(openFrom);
        }


        private void CreateWindow()
        {
            window = new GroupedWindow(new WindowConfig()
                .title("SOCOTEC Conflict Checks")
                .iconCls("icon-users")
                .collapsible(true)
                .resizable(true)
                .minimizable(true)
                .constrainHeader(false)
                .id(windowID)
                .tbar(CreateToolbar())
                .expandOnShow(true)
                .height(650)
                .width(1000)
                .items(new object[] { expertWitnessGrid, expertWitnessGrid2 })
                .ToDictionary());

            window.on(WindowEvents.resize, new WindowResizeDelegate(delegate { expertWitnessGrid.setSize(window.getInnerWidth(), 250); }));
            window.on(WindowEvents.resize, new WindowResizeDelegate(delegate { expertWitnessGrid2.setSize(window.getInnerWidth(), window.getInnerHeight()); }));
            window.render(Global.Desktop.getEl());
            TaskButtonsManager taskButtonsManager = new TaskButtonsManager(window);
            taskButtonsManager.AddWindow();
        }
     
        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Creates Tool Bar

        private object[] CreateToolbar()
        {
            btnRemoveConflictCheck = new Button(new ButtonConfig()                                               // Remove Conflict Check BUtton
                    .text("Remove Conflict Check")                                                                                  // Calls RemoveNewConflictCheck Method
                    .tooltip(new Dictionary("title", "Remove", "text", "Remove the Selected Conflict Check.",
                        "animate", true, "trackMouse", true))
                    .handler(new Callback(RemoveNewConflictCheck))
                    .iconCls("icon-delete")
                    .scope(this)
                    .ToDictionary());

            btnAddConflictCheck = new Button(                                                                    // Add New Conflict Check BUtton
                new ButtonConfig()                                                                               // Calls AddNewConflictCheck Method Window/Panel
                    .text("Initiate New Conflict Check")
                    .tooltip(new Dictionary("title", "Initiate New Conflict Check", "text", "Add a selected Conflict Check.", "animate", true, "trackMouse", true))
                    .handler(new Callback(AddNewConflictCheck))
                    .iconCls("icon-location-add")
                    .scope(this)
                    .ToDictionary()
                );

            return new object[]
            {
                btnAddConflictCheck, "-", btnRemoveConflictCheck
            };
        }
        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // AddNewConflictCheck Method
        // Called in btnAddConflictCheck
        // Calls to Expert Witness Form
        private void AddNewConflictCheck()
        {
            //new ExpertWitnessForm().ShowWindow(Element.get(Document.GetElementById("ExpertWitnessForm")));
            ExpertWitnessForm2 form = new ExpertWitnessForm2();
            form.on("windowClosed", new AddChildDelegate(delegate (GroupedWindow childWindow, ParentClosingBehaviors closingBehavior)
            {
                console.info("WindowClosed");
                expertWitnessStore.reload();
            }));
            form.init(Element.get(Document.GetElementById("ExpertWitnessForm")));
        }
        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // RemoveNewConflictCheck Method
        // Called in btnRemoveConflictCheck
        private void RemoveNewConflictCheck()
        {
            Record row = ((RowSelectionModel)expertWitnessGrid.getSelectionModel()).getSelected();

            MessageBox.confirm("Confirm Delete", "Are you sure you want to delete the selected Conflict Check?", new MessageBoxResponseDelegate(
                delegate (string button, string text)
                {
                    if (button != "yes") return;
                    MessageBox.wait("Deleting selected Conflict Check, please wait.", "Deleting");
                    Dictionary ajaxOptions = new Dictionary();
                    ajaxOptions["url"] = "DataHandler.ashx";
                    ajaxOptions["params"] = new Dictionary(
                        "requestType", "Misc",
                        "requestAction", "DeleteForm",
                        "ExpertWitnessFormID", row.get("ExpertWitnessFormID")
                        );
                    ajaxOptions["callback"] = new AjaxCallbackDelegate(delegate (Dictionary opt, bool success, XMLHttpRequest response)
                    {
                        JsonResponse parsedResponse = JsonResponse.Parse(response.ResponseText);
                        if (!parsedResponse.Success)
                        {
                            MessageBox.alert("There was an error while deleting the conflict Check from the server.");
                        }
                        else
                        {
                            MessageBox.alert("The conflict Check has Sucessfully been deleted from the server.");
                            expertWitnessStore.reload();
                        }
                    });
                    Ajax.request(ajaxOptions);
                }));

        }

        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // My Conflict Checks Panel
        private void CreateGrid()
        {
            expertWitnessStore = new JsonStore(new JsonStoreConfig()
                                            .url("DataHandler.ashx")
                                            .custom("root", "Data")
                                            .custom("totalProperty", "TotalCount")
                                            .baseParams(new Dictionary("requestType", "Misc", "requestAction", "GetAllForms"))
                                            .fields(new object[]
                                                     {
                                                       "ExpertWitnessFormID", "EnteredDate", "UserID", "ProjectName", "Status", "Values"
                                                     })
                                            .ToDictionary());
            expertWitnessStore.load();

            error = new Label(new LabelConfig()
                .text("")
                .style("color: #FF0000; font-weight:bold; padding-left: 7px;")
                .ToDictionary());

            btnClear = new Button(new ButtonConfig()
                    .text("Clear Search")
                    .iconCls("icon-clear")
                    .scope(this)
                    .ToDictionary());

            PagingToolbar pagingBar = new PagingToolbar(new PagingToolbarConfig()
                .store(expertWitnessStore)
                .pageSize(50)
                .displayInfo(true)
                .items(new object[] { error })
                .displayMsg("Showing Requests {0}\u200A\u2013\u200A{1} of {2}")
                .emptyMsg("No requests to display")
                .ToDictionary());

            customFieldExpandedPanel = new Panel(new PanelConfig()
                .bodyBorder(false)
                .border(false)
                .ToDictionary());

           

            ColumnModel colModel = new ColumnModel(new ColumnModelConfig().columns(new object[]
            {
                //expander,
                new ColumnConfig().header("Date Initiated").width(150).sortable(false).renderer(Util.ColumnDateTimeObjectRenderEW("EnteredDate")).ToDictionary(),
                new ColumnConfig().header("Project Name").dataIndex("ProjectName").width(150).sortable(false).ToDictionary(),
                new ColumnConfig().header("Conflict Check Status").dataIndex("Status").width(150).sortable(false).ToDictionary(),
                new ColumnConfig().header("Records").dataIndex("Values").width(150).sortable(false).ToDictionary(),
                //new ColumnConfig().header("Contact Info").dataIndex("ContactInfo").width(120).sortable(false).ToDictionary(),

            }).ToDictionary());
            ColumnModel colModel2 = new ColumnModel(new ColumnModelConfig().columns(new object[]
            {
                //expander,
                new ColumnConfig().header("Date Initiated").width(150).sortable(false).renderer(Util.ColumnDateTimeObjectRenderEW("EnteredDate")).ToDictionary(),
                new ColumnConfig().header("Project Name").dataIndex("ProjectName").width(150).sortable(false).ToDictionary(),
                new ColumnConfig().header("Conflict Check Status").dataIndex("Status").width(150).sortable(false).ToDictionary(),
                new ColumnConfig().header("Records").dataIndex("Values").width(150).sortable(false).ToDictionary(),
                //new ColumnConfig().header("Contact Info").dataIndex("ContactInfo").width(120).sortable(false).ToDictionary(),

            }).ToDictionary());


            GridView gridView = new GridView(new GridViewConfig().autoFill(false).forceFit(true).ToDictionary());
            expertWitnessGrid = new GridPanel(new GridPanelConfig()
                .title("My Conflict Checks")
                .store(expertWitnessStore)
                .colModel(colModel)
                .stripeRows(true)
                .bbar(pagingBar)
                //.tbar(new object[] { customFieldSearch, "-", addressSearch, "-", clientInfoSearch, "-", contactInfoSearch, "->", btnClear })
                .loadMask(true)
                .enableColumnMove(false)
                .trackMouseOver(true)
                //.width(150)
                .view(gridView)
                .border(true)
                .ToDictionary());


            GridView gridView2 = new GridView(new GridViewConfig().autoFill(false).forceFit(true).ToDictionary());
            expertWitnessGrid2 = new GridPanel(new GridPanelConfig()
                .title("My Required Responses to Conflict Checks")
                .store(expertWitnessStore)
                .colModel(colModel2)
                .stripeRows(true)
                .bbar(pagingBar)
                //.tbar(new object[] { customFieldSearch, "-", addressSearch, "-", clientInfoSearch, "-", contactInfoSearch, "->", btnClear })
                .loadMask(true)
                .enableColumnMove(false)
                .trackMouseOver(true)
                //.width(150)
                .view(gridView2)
                .border(true)
                .ToDictionary());

        }
        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ClearError()
        {
            error.setText("");
        }
        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void RefreshCustomFieldExpandedPanelData(Panel p, double rowIndex)
        {
            string html = "<hr>";

            //input the date requested and the request notes
            html += "<div style=\"padding-left: 3px;\"><b>Date Requested</b>: " + expertWitnessStore.getAt(rowIndex).get("RequestedOn") + "<br>";

            //to prevent the styling from breaking if no notes exists...
            if (!Script.IsNullOrUndefined(expertWitnessStore.getAt(rowIndex).get("Notes")))
            {
                html += "<b>Notes</b>: " + expertWitnessStore.getAt(rowIndex).get("Notes") + "</div><br>";
            }
            else
            {
                html += "</div><br>";
            }

            if (expertWitnessStore.getAt(rowIndex).get("CompanyCustomFields") != null)
            {
                displayValues = (Dictionary)(((Dictionary)(JSON.decode(expertWitnessStore.getAt(rowIndex).get("CompanyCustomFields").ToString())))["DisplayValues"]);
                html += "<ul>";

                foreach (DictionaryEntry dictionaryEntry in displayValues)
                {
                    html = html + "<div style=\"list-style: initial; padding: 0px 0px 5px 20px;\"><li><b>" + dictionaryEntry.Key + "</b>: " + dictionaryEntry.Value + "</li></div>";
                }
                html += "</ul>";

            }
            else
            {
                html = html + "<div style=\"padding-left: 3px;\"><i>No Custom Fields</i></div><br>";
            }

            html += "<hr>";
            p.getEl().update(html);
            p.doLayout();
        }
        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
       
        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    }
}

