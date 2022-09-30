using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web.SessionState;
using Versentia.Web.Data;
using Versentia.Web.Models;
using Versentia.Web.Models.Billing;
using Versentia.Web.Models.Schedules;
using Versentia.Web.Models.Services;
using Versentia.Web.Utils;
using Versentia.Web.Utils.Security;

namespace Versentia.Web.Handlers
{
	internal class MiscHandler : HandlerBase
	{
		public MiscHandler(RequestData data, HttpSessionState session, int currentUserID) : base(data, session, currentUserID)
		{
		}

		public JsonResponse SubmitExpertWitnessForm()
		{
			var Parameters = new ExpertWitnessParams();
			Parameters.LeadGenerator = data.GetString("LeadGenerator");
			Parameters.Project = data.GetString("Project");
			Parameters.ProjectAddress = data.GetString("ProjectAddress", false);
			Parameters.QualificationsSubmitted = data.GetString("QualificationsSubmitted", false);
			Parameters.DateProposed = data.GetString("DateProposed", false);
			Parameters.ProposalDue = data.GetString("ProposalDue", false);
			Parameters.ReferredLead = data.GetString("ReferredLead", false);
			Parameters.NameEntity = data.GetString("NameEntity", false);
			Parameters.TypeEntity = data.GetString("TypeEntity", false);
			Parameters.ClientContact = data.GetString("ClientContact", false);
			Parameters.ClientCounsel = data.GetString("ClientCounsel", false);
			Parameters.UltimateBeneficiary = data.GetString("UltimateBeneficiary", false);
			Parameters.ScopeType = data.GetString("ScopeType", false);
			Parameters.ApproximateFee = data.GetString("ApproximateFee", false);
			Parameters.SizeClaim = data.GetString("SizeClaim", false);
			Parameters.OpposingParty = data.GetString("OpposingParty", false);
			Parameters.OpposingLawFirm = data.GetString("OpposingLawFirm", false);
			Parameters.OpposingExpert = data.GetString("OpposingExpert", false);
			Parameters.RelatedParties = data.GetString("RelatedParties", false);
			Parameters.ApproximateStart = data.GetString("ApproximateStart", false);
			Parameters.ApproximateDuration = data.GetString("ApproximateDuration", false);
			Parameters.Representee = data.GetString("Representee", false);
			Parameters.RepresenteeType = data.GetString("RepresenteeType", false);
			Parameters.Source = data.GetString("Source", false);
			Parameters.Probability = data.GetString("Probability", false);
			Parameters.MarketSegment = data.GetString("MarketSegment", false);
			Parameters.EndMarket = data.GetString("EndMarket", false);
			Parameters.Dispute = data.GetString("Dispute", false);
			Parameters.Advisory = data.GetString("Advisory", false);


			var result = MiscDataAccess.SubmitExpertWitnessForm(Parameters, currentUserID);
			return new JsonResponse(result);
		}

		public JsonResponse GetAllForms()
		{
			var result = MiscDataAccess.GetAllForms();
			return new JsonResponse(result);
		}

        public JsonResponse DeleteForm()
        {
            var result = MiscDataAccess.DeleteExpertWitnessForm(data.GetInt("ExpertWitnessFormID"), currentUserID);
            return new JsonResponse(result);
        }
    }

	public class ExpertWitnessParams
    {
		public string LeadGenerator { get; set; }
		public string Project { get; set; }
		public string ProjectAddress { get; set; }
		public string QualificationsSubmitted { get; set; }
		public string DateProposed { get; set; }
		public string ProposalDue { get; set; }
		public string ReferredLead { get; set; }
		public string NameEntity { get; set; }
		public string TypeEntity { get; set; }
		public string ClientContact { get; set; }
		public string ClientCounsel { get; set; }
		public string UltimateBeneficiary { get; set; }
		public string ScopeType { get; set; }
		public string ApproximateFee { get; set; }
		public string SizeClaim { get; set; }
		public string OpposingParty { get; set; }
		public string OpposingLawFirm { get; set; }
		public string OpposingExpert { get; set; }
		public string RelatedParties { get; set; }
		public string ApproximateStart { get; set; }
		public string ApproximateDuration { get; set; }
		public string Representee { get; set; }
		public string RepresenteeType { get; set; }
		public string Source { get; set; }
		public string Probability { get; set; }
		public string MarketSegment { get; set; }
		public string EndMarket { get; set; }
		public string Dispute { get; set; }
		public string Advisory { get; set; }

	}
}
