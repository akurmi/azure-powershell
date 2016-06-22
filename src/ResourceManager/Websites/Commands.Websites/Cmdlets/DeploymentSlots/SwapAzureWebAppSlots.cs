
// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------


using Microsoft.Azure.Commands.WebApps.Utilities;
using System.Management.Automation;

namespace Microsoft.Azure.Commands.WebApps.Cmdlets.DeploymentSlots
{
    /// <summary>
    /// this commandlet will let you swap two web app slots using ARM APIs
    /// </summary>
    [Cmdlet("Swap", "AzureRmWebAppSlots")]
    public class SwapAzureWebAppSlots : WebAppBaseCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "Name of the source slot.", ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string SourceSlotName { get; set; }

        [Parameter(Position = 1, Mandatory = false, HelpMessage = "Name of the destination slot.")]
        [ValidateNotNullOrEmpty]
        public string DestinationSlotName { get; set; }

        [Parameter(Position = 2, Mandatory = false, HelpMessage = "Swap with preview action to use", ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public SwapWithPreviewAction? SwapWithPreviewAction { get; set; }

        [Parameter(Position = 3, Mandatory = false, HelpMessage = "Flag to determine if VNet should be preserved", ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public bool? PreserveVnet { get; set; }

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();
            if (!SwapWithPreviewAction.HasValue)
            {
                WebsitesClient.SwapSlot(ResourceGroupName, Name, SourceSlotName, DestinationSlotName, PreserveVnet);
            }
            else
            {
                switch (SwapWithPreviewAction.Value)
                {
                    case Utilities.SwapWithPreviewAction.ApplySlotConfig:
                        WebsitesClient.SwapSlotWithPreviewApplySlotConfig(ResourceGroupName, Name, SourceSlotName, DestinationSlotName, PreserveVnet);
                        break;
                    case Utilities.SwapWithPreviewAction.CompleteSlotSwap:
                        WebsitesClient.SwapSlot(ResourceGroupName, Name, SourceSlotName, DestinationSlotName, PreserveVnet);
                        break;
                    case Utilities.SwapWithPreviewAction.ResetSlotSwap:
                        WebsitesClient.SwapSlotWithPreviewResetSlotSwap(ResourceGroupName, Name, SourceSlotName);
                        break;
                }
            }
        }
    }
}
