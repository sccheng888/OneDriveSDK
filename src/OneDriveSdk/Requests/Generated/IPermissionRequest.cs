// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

// **NOTE** This file was generated by a tool and any changes will be overwritten.


namespace Microsoft.OneDrive.Sdk
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Graph;

    /// <summary>
    /// The interface IPermissionRequest.
    /// </summary>
    public partial interface IPermissionRequest : IBaseRequest
    {
        /// <summary>
        /// Creates the specified Permission using PUT.
        /// </summary>
        /// <param name="permissionToCreate">The Permission to create.</param>
        /// <returns>The created Permission.</returns>
        Task<Permission> CreateAsync(Permission permissionToCreate);        /// <summary>
        /// Creates the specified Permission using PUT.
        /// </summary>
        /// <param name="permissionToCreate">The Permission to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The created Permission.</returns>
        Task<Permission> CreateAsync(Permission permissionToCreate, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the specified Permission.
        /// </summary>
        /// <returns>The task to await.</returns>
        Task DeleteAsync();

        /// <summary>
        /// Deletes the specified Permission.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The task to await.</returns>
        Task DeleteAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the specified Permission.
        /// </summary>
        /// <returns>The Permission.</returns>
        Task<Permission> GetAsync();

        /// <summary>
        /// Gets the specified Permission.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The Permission.</returns>
        Task<Permission> GetAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Updates the specified Permission using PATCH.
        /// </summary>
        /// <param name="permissionToUpdate">The Permission to update.</param>
        /// <returns>The updated Permission.</returns>
        Task<Permission> UpdateAsync(Permission permissionToUpdate);

        /// <summary>
        /// Updates the specified Permission using PATCH.
        /// </summary>
        /// <param name="permissionToUpdate">The Permission to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The updated Permission.</returns>
        Task<Permission> UpdateAsync(Permission permissionToUpdate, CancellationToken cancellationToken);

        /// <summary>
        /// Adds the specified expand value to the request.
        /// </summary>
        /// <param name="value">The expand value.</param>
        /// <returns>The request object to send.</returns>
        IPermissionRequest Expand(string value);

        /// <summary>
        /// Adds the specified select value to the request.
        /// </summary>
        /// <param name="value">The select value.</param>
        /// <returns>The request object to send.</returns>
        IPermissionRequest Select(string value);

    }
}
