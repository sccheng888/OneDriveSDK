// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

// **NOTE** This file was generated by a tool and any changes will be overwritten.


namespace Microsoft.OneDrive.Sdk
{
    using System;

    using Microsoft.Graph;
    
    /// <summary>
    /// The type DriveSpecialCollectionPage.
    /// </summary>
    public partial class DriveSpecialCollectionPage : CollectionPage<Item>, IDriveSpecialCollectionPage
    {
        /// <summary>
        /// Gets the next page <see cref="IDriveSpecialCollectionRequest"/> instance.
        /// </summary>
        public IDriveSpecialCollectionRequest NextPageRequest { get; private set; }

        /// <summary>
        /// Initializes the NextPageRequest property.
        /// </summary>
        public void InitializeNextPageRequest(IBaseClient client, string nextPageLinkString)
        {
            if (!string.IsNullOrEmpty(nextPageLinkString))
            {
                this.NextPageRequest = new DriveSpecialCollectionRequest(
                    nextPageLinkString,
                    client,
                    null);
            }
        }
    }
}
