// ///////////////////////////////////////////////////////////////////////

// ExpertWitnessWindow.cs
// Calls from ExpertWitnessAdmin Method --> ExpertWitnessForm
// Sonny Sparks

// ///////////////////////////////////////////////////////////////////////

using System;
using Ext;
using Ext.data;
using Ext.form;
using Ext.grid;
using Ext.util;
using FireSharp;
using Versentia.Web.Scripts.UI;
using Versentia.Web.Scripts.Utils;

namespace Versentia.Web.Scripts.Accounts
{
	public class ExpertWitnessForm2 : Observable
	{
		public ExpertWitnessForm2()
        {
			addEvents(new Dictionary(
						  "windowCreated", true,
						  "windowClosed", true
						  ));
		}
		private GroupedWindow window;
		private readonly FormDimensions fd = new FormDimensions(250, 300, 150);

		private FieldSet ConflictFieldSet;
		private int AddressID;
		private FieldSet DidYouMeanFieldSet;
		private Panel ConflictPanel;
		private int LocationID;
		private Button btnSubmit;
		private bool editing;
		private FormPanel formPanel;
		private GoogleAddressValidator googleAddressValidator;
		private StreetAddress orignalAddress;
		private Record primaryAdd;
		private Record record;
		private string windowID;
		private int windowReference;
		private ConflictEntryFieldSet conflictEntryFieldSet;


		// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Creates Overall Window
		//Called from ExpertWitnessWindow.cs
		public void init(Element openFrom)
		{
			conflictEntryFieldSet = new ConflictEntryFieldSet(primaryAdd, false);

			SetupForm();
			CreateWindow();
			window.animateTarget = openFrom;
			window.show();
			
		}
		// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void CreateWindow()
		{
			btnSubmit = new Button(new ButtonConfig()
				.text("Submit Form")
				.handler(new Callback(SubmitForm))
				.ToDictionary());
			window = new GroupedWindow(new WindowConfig()
				.title("SOCOTEC Conflict Checks Form")
				.iconCls("icon-users")
				.collapsible(true)
				.resizable(false)
				.expandOnShow(true)
				.minimizable(true)
				.id(windowID)
				.constrainHeader(true)
				.autoHeight(true)
				.width(650)
				.padding(15)
				.items(formPanel)
				.bbar(new object[]
				{
					new Button(new ButtonConfig().text("Cancel").handler(new Callback(delegate { window.close(); })).ToDictionary()),
					"->",
					btnSubmit
				})
				.ToDictionary()
				);

			window.render(Global.Desktop.getEl());
			TaskButtonsManager taskButtonsManager = new TaskButtonsManager(window);
			taskButtonsManager.AddWindow();
			fireEvent("windowCreated", window);
		}

        private void SubmitForm()
        {
			if (!formPanel.getForm().isValid()) return;
			formPanel.form.submit(new FormSubmitConfig()
				.type("Misc")
				.action("SubmitExpertWitnessForm")
				.waitMsg("Submitting the Form")
				.failure(
                    delegate (FormPanel form, ActionClass action)
					{
						JsonResponse r = JsonResponse.Parse(action.result);
						if (r.Errors.Length == 0 && !Script.IsNullOrUndefined(r.GeneralError))
						{
							MessageBox.alert("Error", r.GeneralError);
						}
					}
				)
				.success(
					delegate (FormPanel form, ActionClass action)
					{
						JsonResponse r = JsonResponse.Parse(action.result);
						if ((bool)(r.Data))
                        {
							MessageBox.alert("Submitted","The Form has been submitted Successfully");
							fireEvent("windowClosed", window);
							window.close();
						}
                        else
                        {
							MessageBox.alert("Error","There was an error submitting the form.");
						}
						console.info("Return Data", r.Data);
					}
				)
				.ToDictionary()
				);
		}


        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void SetupForm()
		{
			ConflictPanel = new Panel(new PanelConfig()
				.border(false)
				.frame(false)
				.autoHeight(true)
				.autoWidth(true)
				.cls("formatted-addresses")
				.style("max-height:250px; overflow: auto;")
				.ToDictionary());

			ConflictFieldSet = conflictEntryFieldSet.ConflictFieldSet;
			ConflictFieldSet.insert(0, ConflictPanel);
			object[] ConflictFields;
			

			formPanel = new FormPanel(new FormPanelConfig()
				.labelAlign("left")
				.frame(false)
				.labelWidth(fd.labelWidth)
				.width(fd.formWidth)
				.autoHeight(true)
				.margins(10)
				.items(new object[] { ConflictFieldSet })
				.ToDictionary());
		}
		// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	}
	// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public class ConflictEntryFieldSet : Observable
	{
		public FieldSet ConflictFieldSet;

		public TextField LeadGenerator;
		public TextField Project;
		public TextField ProjectAddress;
		public TextField QualificationsSubmitted;
		public TextField DateProposed;
		public TextField ProposalDue;
		public TextField ReferredLead;
		public TextField NameEntity;
		public TextField TypeEntity;
		public TextField ClientContact;
		public TextField ClientCounsel;
		public TextField UltimateBeneficiary;
		public TextField ScopeType;
		public TextField ApproximateFee;
		public TextField SizeClaim;
		public TextField OpposingParty;
		public TextField OpposingLawFirm;
		public TextField OpposingExpert;
		public Button RelatedParties;
		public TextField Representee;
		public TextField Comments;

		public Checkbox Dispute;
		public Checkbox Advisory;
		public Checkbox Aggreement;
		public Checkbox Plaintiff;
		public Checkbox Defendant;
		public Checkbox NA;
		public Checkbox ClaimsAssistance;
		public Checkbox ExpertService;
		public Checkbox FinancialAnalysis;
		public Checkbox ProjectManagement;
		public Checkbox Scheduling;
		public Checkbox Investigation;

		public DateField ApproximateStart;
		public DateField ApproximateDuration;

		public ComboBox RepresenteeType;
		public ComboBox Source;
		public ComboBox Probability;
		public ComboBox MarketSegment;
		public ComboBox EndMarket;
		//private ComboBox ApproximateStart;
		//private ComboBox ApproximateDuration;

		private readonly Store RepresentingTypeStore = new Store(new StoreConfig()
			.reader(new ArrayReader(new Dictionary(), new Dictionary[] { new Dictionary("name", "Name") }))
			.ToDictionary()
			);
		private readonly Store SourceTypeStore = new Store(new StoreConfig()
			.reader(new ArrayReader(new Dictionary(), new Dictionary[] { new Dictionary("name", "Name") }))
			.ToDictionary()
			);
		private readonly Store ProbabilityTypeStore = new Store(new StoreConfig()
			.reader(new ArrayReader(new Dictionary(), new Dictionary[] { new Dictionary("name", "Name") }))
			.ToDictionary()
			);
		private readonly Store MarketTypeStore = new Store(new StoreConfig()
			.reader(new ArrayReader(new Dictionary(), new Dictionary[] { new Dictionary("name", "Name") }))
			.ToDictionary()
			);
		private readonly Store EndMarkTypeStore = new Store(new StoreConfig()
			.reader(new ArrayReader(new Dictionary(), new Dictionary[] { new Dictionary("name", "Name") }))
			.ToDictionary()
			);
		private bool condenseUI;

		// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public ConflictEntryFieldSet(Record primaryAddress, bool forAddLocaiton)
		{
			addEvents(new Dictionary(
				"changeMade", true,
				"update", true,
				"windowCreated", true
				));
			MakeStoreValues();
			SetupFieldSet();

		}
		// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		private void SetupFieldSet()
		{
			Dispute = new Checkbox(new CheckboxConfig().fieldLabel("Dispute").checked_(false).name("Dispute").ctCls("expertwitness-checkbox").labelStyle("width: unset").style("margin-left: 50px")
				.ToDictionary());
			Advisory = new Checkbox(new CheckboxConfig().fieldLabel("Advisory").checked_(false).name("Advisory").ctCls("expertwitness-checkbox").labelStyle("width: unset").style("margin-left: 50px")
				.ToDictionary());
            // /////////////////////////////////////////////////////////////

            Dictionary formColumnz = new Dictionary("layout", "column", "border", false, "items",
                new Dictionary[]
                    {
						new Dictionary("layout", "form", "columnWidth", .5, "border", false, "items", new object []{Dispute}),
						new Dictionary("layout", "form", "columnWidth", .5, "style", "margin-left: 10px", "border", false, "items", new object []{Advisory})
                    });
            FieldSet DisputeAdvisory = new FieldSet(new FieldSetConfig()
                .autoWidth(true).autoHeight(true).frame(false).border(true).items(formColumnz).ToDictionary());
			// /////////////////////////////////////////////////////////////

			LeadGenerator = new TextField(new TextFieldConfig().fieldLabel("Lead Generator").name("LeadGenerator").allowBlank(true).enableKeyEvents(true)
				.ToDictionary());
			Project = new TextField(new TextFieldConfig().fieldLabel("Project").name("Project").allowBlank(true).enableKeyEvents(true)
				.ToDictionary());
			ProjectAddress = new TextField(new TextFieldConfig().fieldLabel("Project Address").name("ProjectAddress").allowBlank(true).enableKeyEvents(true)
				.ToDictionary());
			QualificationsSubmitted = new TextField(new TextFieldConfig().fieldLabel("Qualifications Submitted").name("QualificationsSubmitted").allowBlank(true).enableKeyEvents(true)
				.ToDictionary());
			DateProposed = new TextField(new TextFieldConfig().fieldLabel("Agreement sent").name("DateProposed").allowBlank(true).enableKeyEvents(true)
				.ToDictionary());
			ProposalDue = new TextField(new TextFieldConfig().fieldLabel("Proposal Due").name("ProposalDue").allowBlank(true).enableKeyEvents(true)
				.ToDictionary());
			ReferredLead = new TextField(new TextFieldConfig().fieldLabel("Referred Lead").name("ReferredLead").allowBlank(true).enableKeyEvents(true)
				.ToDictionary());
			NameEntity = new TextField(new TextFieldConfig().fieldLabel("Name of entity").name("NameEntity").allowBlank(true).enableKeyEvents(true).labelStyle("margin-left: 50px")
				.ToDictionary());
			TypeEntity = new TextField(new TextFieldConfig().fieldLabel("Type of Entity").name("TypeEntity").allowBlank(true).enableKeyEvents(true).labelStyle("margin-left: 50px")
				.ToDictionary());
			Representee = new TextField(new TextFieldConfig().fieldLabel("Who are we Representing").name("Representee").allowBlank(true).enableKeyEvents(true)
				.ToDictionary());
			ClientContact = new TextField(new TextFieldConfig().fieldLabel("Client Contact").name("ClientContact").allowBlank(true).enableKeyEvents(true).labelStyle("margin-left: 50px")
				.ToDictionary());
			ClientCounsel = new TextField(new TextFieldConfig().fieldLabel("Client Counsel").name("ClientCounsel").allowBlank(true).enableKeyEvents(true).labelStyle("margin-left: 50px")
				.ToDictionary());
			UltimateBeneficiary = new TextField(new TextFieldConfig().fieldLabel("Ultimate Benificiary").name("UltimateBenificiary").allowBlank(true).enableKeyEvents(true)
				.ToDictionary());
			ScopeType = new TextField(new TextFieldConfig().fieldLabel("Scope Type").name("ScopeType").allowBlank(true).enableKeyEvents(true)
				.ToDictionary());
			ApproximateFee = new TextField(new TextFieldConfig().fieldLabel("Approximate Fee").name("ApproximateFee").allowBlank(true).enableKeyEvents(true)
				.ToDictionary());
			SizeClaim = new TextField(new TextFieldConfig().fieldLabel("Size Claim").name("SizeClaim").allowBlank(true).enableKeyEvents(true)
				.ToDictionary());
			OpposingParty = new TextField(new TextFieldConfig().fieldLabel("Opposing Party").name("OpposingParty").allowBlank(true).enableKeyEvents(true)
				.ToDictionary());
			OpposingLawFirm = new TextField(new TextFieldConfig().fieldLabel("Opposing Law Firm").name("OpposingLawFirm").allowBlank(true).enableKeyEvents(true)
				.ToDictionary());
			OpposingExpert = new TextField(new TextFieldConfig().fieldLabel("Opposing Experts").name("OpposingExperts").allowBlank(true).enableKeyEvents(true)
				.ToDictionary());
			Comments = new TextField(new TextFieldConfig().fieldLabel("Comments Notes").name("CommentsNotes").allowBlank(true).enableKeyEvents(true)
				.height(50)
				.ToDictionary());
			// /////////////////////////////////////////////////////////////
			ApproximateStart = new DateField(new DateFieldConfig()
				.fieldLabel("Anticipated Start Date")
				.allowBlank(true)
				.format("m/d/y")
				.width(175)
				.ToDictionary());
			ApproximateDuration = new DateField(new DateFieldConfig()
				.fieldLabel("Anticipated End Date")
				.allowBlank(true)
				.format("m/d/y")
				.width(175)
				.ToDictionary());
			RepresenteeType = new ComboBox(new ComboBoxConfig()
				.fieldLabel("RepresentingType")
				.store(RepresentingTypeStore)
				.forceSelection(true)
				.name("RepresentingType")
				.hiddenName("IdentifierType")
				.displayField("Name")
				.valueField("Name")
				.typeAhead(true)
				.editable(false)
				.width(195)
				.allowBlank(true)
				.enableKeyEvents(false)
				.disabled(false)
				.triggerAction("all")
				.style("display: inline; clear: none;")
				.mode("local")
				.ToDictionary()
				);
			Source = new ComboBox(new ComboBoxConfig()
				.fieldLabel("Source")
				.store(SourceTypeStore)
				.forceSelection(true)
				.name("Source")
				.hiddenName("IdentifierType")
				.displayField("Name")
				.valueField("Name")
				.typeAhead(true)
				.editable(false)
				.width(195)
				.allowBlank(true)
				.enableKeyEvents(false)
				.disabled(false)
				.triggerAction("all")
				.style("display: inline; clear: none;")
				.labelStyle("margin-left: 50px")
				.itemCls("expertwitness-combobox")
				.mode("local")
				.ToDictionary()
				);
			Probability = new ComboBox(new ComboBoxConfig()
				.fieldLabel("Probability")
				.store(ProbabilityTypeStore)
				.forceSelection(true)
				.name("Probability")
				.hiddenName("IdentifierType")
				.displayField("Name")
				.valueField("Name")
				.typeAhead(true)
				.editable(false)
				.width(175)
				.allowBlank(true)
				.enableKeyEvents(false)
				.disabled(false)
				.triggerAction("all")
				.style("display: inline; clear: none;")
				.mode("local")
				.ToDictionary()
				);
			MarketSegment = new ComboBox(new ComboBoxConfig()
				.fieldLabel("Market Segment")
				.store(MarketTypeStore)
				.forceSelection(true)
				.name("Market Segment")
				.hiddenName("IdentifierType")
				.displayField("Name")
				.valueField("Name")
				.typeAhead(true)
				.editable(false)
				.width(195)
				.allowBlank(true)
				.enableKeyEvents(false)
				.disabled(false)
				.triggerAction("all")
				.style("display: inline; clear: none;")
				//.labelStyle("margin-left: -150px")
				//.ctCls("expertwitness-combobox")
				.mode("local")
				.ToDictionary()
				);
			EndMarket = new ComboBox(new ComboBoxConfig()
				.fieldLabel("End Market")
				.store(EndMarkTypeStore)
				.forceSelection(true)
				.name("End Market")
				.hiddenName("IdentifierType")
				.displayField("Name")
				.valueField("Name")
				.typeAhead(true)
				.editable(false)
				.width(195)
				.allowBlank(true)
				.enableKeyEvents(false)
				.disabled(false)
				.triggerAction("all")
				.style("display: inline; clear: none;")
				//.labelStyle("margin-left: -150px")
				//.ctCls("expertwitness-combobox")
				.mode("local")
				.ToDictionary()
				);

			RelatedParties = new Button(                                                               
				new ButtonConfig()                                                                               
					.text("Initiate Related Parties Window")
					.tooltip(new Dictionary("title", "Initiate Related Parties Window", "text", "Add a Related Party", "animate", true, "trackMouse", true))
					.handler(new Callback(AddRelatedParties))
					.iconCls("icon-report")
					.style("width: '170px', height: '30px'")
					.fieldLabel("Related Parties")
					.scope(this)
					.ToDictionary()
				);

			// /////////////////////////////////////////////////////////////
			// /////////////////////////////////////////////////////////////
			Dictionary formColumnz2 = new Dictionary("layout", "column", "border", false, "items",
				new Dictionary[]
					{
						new Dictionary("layout", "form", "columnWidth", .5, "border", false, "items", new object []{MarketSegment}),
						new Dictionary("layout", "form", "columnWidth", .5, "style", "margin-left: 10px", "border", false, "items", new object []{EndMarket})
					});
			FieldSet MarketEnd = new FieldSet(new FieldSetConfig()
				.autoWidth(true).autoHeight(true).frame(false).border(true).items(formColumnz2).ToDictionary());
			// /////////////////////////////////////////////////////////////

			LeadGenerator.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			//DisputeAdvisory.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			Dispute.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			Advisory.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			Project.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			ProjectAddress.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			QualificationsSubmitted.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			DateProposed.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			ProposalDue.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			ReferredLead.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			NameEntity.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			TypeEntity.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			ClientContact.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			ClientCounsel.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			UltimateBeneficiary.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			ScopeType.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			ApproximateFee.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			SizeClaim.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			OpposingParty.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			OpposingLawFirm.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			OpposingExpert.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			RelatedParties.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));
			Comments.on(TextFieldEvents.keyup, new TextFieldKeyupDelegate(delegate { fireEvent("changeMade"); }));


			// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			// Collumn Values
			object[] ConflictFields;
			if (Global.CurrentUser.CompanyID == 1 || Global.CurrentUser.CompanyID == 3)
			{
				ConflictFields = condenseUI ? new object[]
				{
				LeadGenerator, DisputeAdvisory, Project, ProjectAddress, QualificationsSubmitted, DateProposed, ProposalDue, ReferredLead, NameEntity, 
					TypeEntity, Source, Representee, RepresenteeType, ClientContact, TypeEntity, ClientCounsel, UltimateBeneficiary, ScopeType, 
						ApproximateFee, Probability, SizeClaim, OpposingParty, OpposingLawFirm, OpposingExpert, RelatedParties, MarketEnd,
							ApproximateStart, ApproximateDuration
							
				} : new object[]
				{
				LeadGenerator, DisputeAdvisory, Project, ProjectAddress, QualificationsSubmitted, DateProposed, ProposalDue, ReferredLead, NameEntity,
					TypeEntity, Source, Representee, RepresenteeType, ClientContact, TypeEntity, ClientCounsel, UltimateBeneficiary, ScopeType,
						ApproximateFee, Probability, SizeClaim, OpposingParty, OpposingLawFirm, OpposingExpert, RelatedParties, MarketEnd,
							ApproximateStart, ApproximateDuration
							
				};
			}
			else
			{
				ConflictFields = condenseUI ? new object[]
				{
				LeadGenerator, DisputeAdvisory, Project, ProjectAddress, QualificationsSubmitted, DateProposed, ProposalDue, ReferredLead, NameEntity,
					TypeEntity, Source, Representee, RepresenteeType, ClientContact, TypeEntity, ClientCounsel, UltimateBeneficiary, ScopeType,
						ApproximateFee, Probability, SizeClaim, OpposingParty, OpposingLawFirm, OpposingExpert, RelatedParties, MarketEnd,
							ApproximateStart, ApproximateDuration
				} : new object[]
				{
				LeadGenerator, DisputeAdvisory, Project, ProjectAddress, QualificationsSubmitted, DateProposed, ProposalDue, ReferredLead, NameEntity,
					TypeEntity, Source, Representee, RepresenteeType, ClientContact, TypeEntity, ClientCounsel, UltimateBeneficiary, ScopeType,
						ApproximateFee, Probability, SizeClaim, OpposingParty, OpposingLawFirm, OpposingExpert, RelatedParties, MarketEnd,
							ApproximateStart, ApproximateDuration
							
				};
			}

			
			if (condenseUI)
			{
				
				LeadGenerator.hideLabel = true;
				Dispute.hideLabel = true;
				Advisory.hideLabel = true;
				Project.hideLabel = true;
				ProjectAddress.hideLabel = true;
				QualificationsSubmitted.hideLabel = true;
				DateProposed.hideLabel = true;
				ProposalDue.hideLabel = true;
				ReferredLead.hideLabel = true;
				NameEntity.hideLabel = true;
				TypeEntity.hideLabel = true;
				Source.hideLabel = true;
				Representee.hideLabel = true;
				ClientContact.hideLabel = true;
				TypeEntity.hideLabel = true;
				ClientCounsel.hideLabel = true;
				UltimateBeneficiary.hideLabel = true;
				ScopeType.hideLabel = true;
				ApproximateFee.hideLabel = true;
				Probability.hideLabel = true;
				SizeClaim.hideLabel = true;
				OpposingParty.hideLabel = true;
				OpposingLawFirm.hideLabel = true;
				OpposingExpert.hideLabel = true;
				RelatedParties.hideLabel = true;
				MarketSegment.hideLabel = true;
				EndMarket.hideLabel = true;
				ApproximateStart.hideLabel = true;
				ApproximateDuration.hideLabel = true;


				LeadGenerator.emptyText = "Lead Generator";
				Project.emptyText = "Project";
				ProjectAddress.emptyText = "Project Address";
				QualificationsSubmitted.emptyText = "Qualifications Submitted";
				DateProposed.emptyText = "Date Proposal Sent";
                ProposalDue.emptyText = "Date Proposal Due";
                NameEntity.emptyText = "Name of Entity";
                TypeEntity.emptyText = "Type of Entity";
                Representee.emptyText = "Who are we Representing?";
                ClientContact.emptyText = "Client Contact";
                TypeEntity.emptyText = "Type of Entity";
                ClientCounsel.emptyText = "Client Counsel";
                UltimateBeneficiary.emptyText = "Ultimate Beneficiary";
                ScopeType.emptyText = "Scope/Type";
                ApproximateFee.emptyText = "Approximate Fee";
				Probability.emptyText = "Probability";
                SizeClaim.emptyText = "Approximate Size of Claim";
                OpposingParty.emptyText = "Opposing Party";
                OpposingLawFirm.emptyText = "Opposing Law Firm";
                OpposingExpert.emptyText = "Opposing Expert";
				MarketSegment.emptyText = "MarketSegment";
				EndMarket.emptyText = "EndMarket";
				ApproximateStart.emptyText = "ApproximateStart";
				ApproximateDuration.emptyText = "ApproximateDuration";
			}

            ConflictFieldSet = new FieldSet(new FieldSetConfig()
				.title("Subject")
				.autoHeight(true)
				.items(ConflictFields)
				.border(!condenseUI)
				.width(600)
				//.buttons(condenseUI ? new Array() : new object[] { btnCopyPrimary })
				.buttonAlign("right")
				.style("margin: 5px 5px 5px 5px;")
				.ToDictionary());



		}

        private void AddRelatedParties()
        {

        }

        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void MakeStoreValues()
		{
			object[] RepresentingTypeValues = new object[] { "N/A", "Plaintiff", "Defendant" };
			object value0 = new object();
			for (int i = 0; i < RepresentingTypeValues.Length; i++)
			{
				value0 = new object[] { new object[] { RepresentingTypeValues[i] } };
				RepresentingTypeStore.loadData(value0, true);
			}

			object[] SourceTypeValues = new object[] { "None", "Repeat", "New" };
			object value1 = new object();
			for (int i = 0; i < SourceTypeValues.Length; i++)
			{
				value1 = new object[] { new object[] { SourceTypeValues[i] } };
				SourceTypeStore.loadData(value1, true);
			}

			object[] ProbabilityTypeValues = new object[] { "None", "0%", "25%", "50%", "75%", "100%" };
			object value2 = new object();
			for (int i = 0; i < ProbabilityTypeValues.Length; i++)
			{
				value2 = new object[] { new object[] { ProbabilityTypeValues[i] } };
				ProbabilityTypeStore.loadData(value2, true);
			}

			object[] MarketTypeValues = new object[] { "None", "General Industry", "Technology", "Automotive" };
			object value3 = new object();
			for (int i = 0; i < MarketTypeValues.Length; i++)
			{
				value3 = new object[] { new object[] { MarketTypeValues[i] } };
				MarketTypeStore.loadData(value3, true);
			}

			object[] EndMarkTypeValues = new object[] { "None", "Healthcare", "Data Science", "Fabrication", "Chemical Processing", "Electrician",
															"Developer", "Manager", "Dog Walker", "Roofer" };
			object value4 = new object();
			for (int i = 0; i < EndMarkTypeValues.Length; i++)
			{
				value4 = new object[] { new object[] { EndMarkTypeValues[i] } };
				EndMarkTypeStore.loadData(value4, true);
			}
		}
		

		public void Reset()
		{
			LeadGenerator.reset();
			LeadGenerator.removeClass("notify-change");
			
		}
	}

}