using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Text.RegularExpressions;
using System.Activities;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ManageNN;

namespace ManageNN
{
    public class Associate : CodeActivity
    {
        #region "Parameter Definition"
        [RequiredArgument]
        [Input("Entity1")]
        public InArgument<String> entity1 { get; set; }

        [RequiredArgument]
        [Input("Entity1 Record URL")]
        public InArgument<String> recordurl { get; set; }

        [RequiredArgument]
        [Input("Entity2")]
        public InArgument<String> entity2 { get; set; }

        [RequiredArgument]
        [Input("Entity2 GUID")]
        public InArgument<String> guid2 { get; set; }

        [RequiredArgument]
        [Input("Relationship")]
        public InArgument<String> NNrelationship { get; set; }

        [Output("Success")]
        public OutArgument<Boolean> success { get;  set; }
        #endregion

        protected override void Execute(CodeActivityContext executionContext)
        {

            #region "Load CRM Service from context"

            Common objCommon = new Common(executionContext);
            objCommon.tracingService.Trace("Load CRM Service from context --- OK");
            #endregion

            #region "Read Parameters"
            String _entity1 = this.entity1.Get(executionContext);
            String _entity2 = this.entity2.Get(executionContext);
            String _nnrelationship = this.NNrelationship.Get(executionContext);
            String _recordurl = this.recordurl.Get(executionContext);
            Regex rx = new Regex("(?<=id=).{36}", RegexOptions.None);
            MatchCollection matches = rx.Matches(_recordurl);
            Guid _guid1 = new Guid(matches[0].Value);
            Guid _guid2 = new Guid(this.guid2.Get(executionContext));
            #endregion


            #region "Execution"
            try
            {
                AssociateRequest request = new AssociateRequest();
                EntityReference moniker1 = new EntityReference(_entity1, _guid1);
                EntityReference moniker2 = new EntityReference(_entity2, _guid2);
                EntityReferenceCollection relatedEntities = new EntityReferenceCollection();
                relatedEntities.Add(moniker1);
                request.Target = moniker2;
                request.RelatedEntities = new EntityReferenceCollection { moniker1 };
                request.Relationship = new Relationship(_nnrelationship);
                objCommon.service.Execute(request);
                success.Set(executionContext, true);
            }
            catch {}
            #endregion
        }
    }
}