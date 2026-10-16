namespace Core.Models.App_Models
{
    public class ApplicationsLIST : List<ApplicationClass>
    {

        public ApplicationsLIST()
        {
        }

        public void Add(string applicationName)
        {
            this.Add(new ApplicationClass
            {
                Name = applicationName,
            });
        }

        public void Add(string applicationName, LOG.CATEGORY category)
        {
            this.Add(new ApplicationClass
            {
                Name = applicationName,
                Category = category
            });
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
                // TODO
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
