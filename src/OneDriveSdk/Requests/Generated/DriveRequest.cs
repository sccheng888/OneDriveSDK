// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

// **NOTE** This file was generated by a tool and any changes will be overwritten.


namespace Microsoft.OneDrive.Sdk
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Graph;

    /// <summary>
    /// The type DriveRequest.
    /// </summary>
    public partial class DriveRequest : BaseRequest, IDriveRequest
    {
        /// <summary>
        /// Constructs a new DriveRequest.
        /// </summary>
        /// <param name="requestUrl">The URL for the built request.</param>
        /// <param name="client">The <see cref="IBaseClient"/> for handling requests.</param>
        /// <param name="options">Query and header option name value pairs for the request.</param>
        public DriveRequest(
            string requestUrl,
            IBaseClient client,
            IEnumerable<Option> options)
            : base(requestUrl, client, options)
        {
            this.SdkVersionHeaderPrefix = "onedrive";
        }

        /// <summary>
        /// Creates the specified Drive using PUT.
        /// </summary>
        /// <param name="driveToCreate">The Drive to create.</param>
        /// <returns>The created Drive.</returns>
        public Task<Drive> CreateAsync(Drive driveToCreate)
        {
            return this.CreateAsync(driveToCreate, CancellationToken.None);
        }

        /// <summary>
        /// Creates the specified Drive using PUT.
        /// </summary>
        /// <param name="driveToCreate">The Drive to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The created Drive.</returns>
        public async Task<Drive> CreateAsync(Drive driveToCreate, CancellationToken cancellationToken)
        {
            this.ContentType = "application/json";
            this.Method = "PUT";
            var newEntity = await this.SendAsync<Drive>(driveToCreate, cancellationToken).ConfigureAwait(false);
            this.InitializeCollectionProperties(newEntity);
            return newEntity;
        }

        /// <summary>
        /// Deletes the specified Drive.
        /// </summary>
        /// <returns>The task to await.</returns>
        public Task DeleteAsync()
        {
            return this.DeleteAsync(CancellationToken.None);
        }

        /// <summary>
        /// Deletes the specified Drive.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The task to await.</returns>
        public async Task DeleteAsync(CancellationToken cancellationToken)
        {
            this.Method = "DELETE";
            await this.SendAsync<Drive>(null, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the specified Drive.
        /// </summary>
        /// <returns>The Drive.</returns>
        public Task<Drive> GetAsync()
        {
            return this.GetAsync(CancellationToken.None);
        }

        /// <summary>
        /// Gets the specified Drive.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The Drive.</returns>
        public async Task<Drive> GetAsync(CancellationToken cancellationToken)
        {
            this.Method = "GET";
            var retrievedEntity = await this.SendAsync<Drive>(null, cancellationToken).ConfigureAwait(false);
            this.InitializeCollectionProperties(retrievedEntity);
            return retrievedEntity;
        }

        /// <summary>
        /// Updates the specified Drive using PATCH.
        /// </summary>
        /// <param name="driveToUpdate">The Drive to update.</param>
        /// <returns>The updated Drive.</returns>
        public Task<Drive> UpdateAsync(Drive driveToUpdate)
        {
            return this.UpdateAsync(driveToUpdate, CancellationToken.None);
        }

        /// <summary>
        /// Updates the specified Drive using PATCH.
        /// </summary>
        /// <param name="driveToUpdate">The Drive to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The updated Drive.</returns>
        public async Task<Drive> UpdateAsync(Drive driveToUpdate, CancellationToken cancellationToken)
        {
            this.ContentType = "application/json";
            this.Method = "PATCH";
            var updatedEntity = await this.SendAsync<Drive>(driveToUpdate, cancellationToken).ConfigureAwait(false);
            this.InitializeCollectionProperties(updatedEntity);
            return updatedEntity;
        }

        /// <summary>
        /// Adds the specified expand value to the request.
        /// </summary>
        /// <param name="value">The expand value.</param>
        /// <returns>The request object to send.</returns>
        public IDriveRequest Expand(string value)
        {
            this.QueryOptions.Add(new QueryOption("$expand", value));
            return this;
        }

        /// <summary>
        /// Adds the specified select value to the request.
        /// </summary>
        /// <param name="value">The select value.</param>
        /// <returns>The request object to send.</returns>
        public IDriveRequest Select(string value)
        {
            this.QueryOptions.Add(new QueryOption("$select", value));
            return this;
        }

        /// <summary>
        /// Initializes any collection properties after deserialization, like next requests for paging.
        /// </summary>
        /// <param name="driveToInitialize">The <see cref="Drive"/> with the collection properties to initialize.</param>
        private void InitializeCollectionProperties(Drive driveToInitialize)
        {

            if (driveToInitialize != null && driveToInitialize.AdditionalData != null)
            {

                if (driveToInitialize.Items != null && driveToInitialize.Items.CurrentPage != null)
                {
                    driveToInitialize.Items.AdditionalData = driveToInitialize.AdditionalData;

                    object nextPageLink;
                    driveToInitialize.AdditionalData.TryGetValue("items@odata.nextLink", out nextPageLink);
                    var nextPageLinkString = nextPageLink as string;

                    if (!string.IsNullOrEmpty(nextPageLinkString))
                    {
                        driveToInitialize.Items.InitializeNextPageRequest(
                            this.Client,
                            nextPageLinkString);
                    }
                }

                if (driveToInitialize.Shared != null && driveToInitialize.Shared.CurrentPage != null)
                {
                    driveToInitialize.Shared.AdditionalData = driveToInitialize.AdditionalData;

                    object nextPageLink;
                    driveToInitialize.AdditionalData.TryGetValue("shared@odata.nextLink", out nextPageLink);
                    var nextPageLinkString = nextPageLink as string;

                    if (!string.IsNullOrEmpty(nextPageLinkString))
                    {
                        driveToInitialize.Shared.InitializeNextPageRequest(
                            this.Client,
                            nextPageLinkString);
                    }
                }

                if (driveToInitialize.Special != null && driveToInitialize.Special.CurrentPage != null)
                {
                    driveToInitialize.Special.AdditionalData = driveToInitialize.AdditionalData;

                    object nextPageLink;
                    driveToInitialize.AdditionalData.TryGetValue("special@odata.nextLink", out nextPageLink);
                    var nextPageLinkString = nextPageLink as string;

                    if (!string.IsNullOrEmpty(nextPageLinkString))
                    {
                        driveToInitialize.Special.InitializeNextPageRequest(
                            this.Client,
                            nextPageLinkString);
                    }
                }

            }


        }
    }
}
