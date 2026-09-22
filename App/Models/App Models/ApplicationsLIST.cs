namespace Data_Logger_1._3.Models.App_Models
{
    public class ApplicationsLIST : List<ApplicationClass>
    {

        public ApplicationsLIST()
        {
        }

        public void Add(string applicationName)
        {
            ApplicationClass app = new(this.Count + 1, applicationName);
            this.Add(app);
        }

        public void Add(string applicationName, LOG.CATEGORY category)
        {
            ApplicationClass app = new(this.Count + 1, applicationName, category);
            this.Add(app);
        }

        public new void Add(ApplicationClass application)
        {

            try
            {
                if (ContainsApplicationWithID(application.appID))
                {
                    return;
                }
            }
            catch (Exception)
            {
                //
            }

            base.Add(application);
        }

        private bool ContainsApplicationWithID(int appId)
        {
            foreach (var app in this)
            {
                if (app.appID == appId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
