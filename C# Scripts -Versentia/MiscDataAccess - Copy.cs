using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Versentia.Web.Handlers;
using Versentia.Web.Models;
using Versentia.Web.Models.Billing;
using Versentia.Web.Models.Conditions;
using Versentia.Web.Models.dbo;
using Versentia.Web.Models.Schedules;
using Versentia.Web.Models.Services;
using Versentia.Web.Utils;
using Versentia.Web.Utils.Security;
using static Versentia.Web.Handlers.MiscHandler;

namespace Versentia.Web.Data
{
	internal class MiscDataAccess
	{
		public static bool SubmitExpertWitnessForm(ExpertWitnessParams Parameters, int currentUserID)
		{
			var context = new VersentiaDataContext();
			var currentUser = CurrentUsers.Instance.GetLoggedInUser(currentUserID);
			var form = new ExpertWitnessForm();
			form.EnteredDate = DateTime.Now;
			form.UserID = currentUserID;
			form.ProjectName = Parameters.Project;
			form.Values = Parameters.Serialize();
			context.ExpertWitnessForms.InsertOnSubmit(form);
			context.SubmitChanges();
			return true;
		}

        public static IEnumerable<ExpertWitnessForm> GetAllForms()
        {
			return new VersentiaDataContext().ExpertWitnessForms.ToList();
        }

        public static bool DeleteExpertWitnessForm(int expertWitnessFormID, int currentUserID)
        {
            var context = new VersentiaDataContext();
            var currentUser = CurrentUsers.Instance.GetLoggedInUser(currentUserID);
			var form = context.ExpertWitnessForms.First(f =>f.ExpertWitnessFormID==expertWitnessFormID);
            context.ExpertWitnessForms.DeleteOnSubmit(form);
            context.SubmitChanges();
            return true;
        }
    }
}