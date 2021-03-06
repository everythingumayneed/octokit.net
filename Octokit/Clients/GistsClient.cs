﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Octokit
{
    /// <summary>
    /// A client for GitHub's Gists API.
    /// </summary>
    /// <remarks>
    /// See the <a href="http://developer.github.com/v3/gists/">Gists API documentation</a> for more information.
    /// </remarks>
    public class GistsClient : ApiClient, IGistsClient
    {
        /// <summary>
        /// Instantiates a new GitHub Gists API client.
        /// </summary>
        /// <param name="apiConnection">An API connection</param>
        public GistsClient(IApiConnection apiConnection) :
            base(apiConnection)
        {
            Comment = new GistCommentsClient(apiConnection);
        }

        public IGistCommentsClient Comment { get; set; }

        /// <summary>
        /// Gets a gist
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#get-a-single-gist
        /// </remarks>
        /// <param name="id">The id of the gist</param>
        public Task<Gist> Get(string id)
        {
            return ApiConnection.Get<Gist>(ApiUrls.Gist(id));
        }

        /// <summary>
        /// Creates a new gist
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#create-a-gist
        /// </remarks>
        /// <param name="newGist">The new gist to create</param>
        public Task<Gist> Create(NewGist newGist)
        {
            Ensure.ArgumentNotNull(newGist, "newGist");

            //Required to create anonymous object to match signature of files hash.  
            // Allowing the serializer to handle Dictionary<string,NewGistFile> 
            // will fail to match.
            var filesAsJsonObject = new JsonObject();
            foreach (var kvp in newGist.Files)
            {
                filesAsJsonObject.Add(kvp.Key, new { Content = kvp.Value });
            }

            var gist = new
            {
                Description = newGist.Description,
                Public = newGist.Public,
                Files = filesAsJsonObject
            };

            return ApiConnection.Post<Gist>(ApiUrls.Gist(), gist);
        }

        /// <summary>
        /// Creates a fork of a gist
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#fork-a-gist
        /// </remarks>
        /// <param name="id">The id of the gist to fork</param>
        public Task<Gist> Fork(string id)
        {
            return ApiConnection.Post<Gist>(ApiUrls.ForkGist(id), new object());
        }

        /// <summary>
        /// Deletes a gist
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#delete-a-gist
        /// </remarks>
        /// <param name="id">The id of the gist</param>
        public Task Delete(string id)
        {
            Ensure.ArgumentNotNullOrEmptyString(id, "id");

            return ApiConnection.Delete(ApiUrls.Gist(id));
        }

        /// <summary>
        /// List the authenticated user’s gists or if called anonymously, 
        /// this will return all public gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        public Task<IReadOnlyList<Gist>> GetAll()
        {
            return GetAll(ApiOptions.None);
        }

        /// <summary>
        /// List the authenticated user’s gists or if called anonymously, 
        /// this will return all public gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        /// <param name="options">Options for changing the API response</param>
        public Task<IReadOnlyList<Gist>> GetAll(ApiOptions options)
        {
            Ensure.ArgumentNotNull(options, "options");

            return ApiConnection.GetAll<Gist>(ApiUrls.Gist(), options);
        }

        /// <summary>
        /// List the authenticated user’s gists or if called anonymously, 
        /// this will return all public gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        /// <param name="since">Only gists updated at or after this time are returned</param>
        public Task<IReadOnlyList<Gist>> GetAll(DateTimeOffset since)
        {
            return GetAll(since, ApiOptions.None);
        }

        /// <summary>
        /// List the authenticated user’s gists or if called anonymously, 
        /// this will return all public gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        /// <param name="since">Only gists updated at or after this time are returned</param>
        /// <param name="options">Options for changing the API response</param>
        public Task<IReadOnlyList<Gist>> GetAll(DateTimeOffset since, ApiOptions options)
        {
            Ensure.ArgumentNotNull(options, "options");

            var request = new GistRequest(since);
            return ApiConnection.GetAll<Gist>(ApiUrls.Gist(), request.ToParametersDictionary(), options);
        }

        /// <summary>
        /// Lists all public gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        public Task<IReadOnlyList<Gist>> GetAllPublic()
        {
            return GetAllPublic(ApiOptions.None);
        }

        /// <summary>
        /// Lists all public gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        /// <param name="options">Options for changing the API response</param>
        public Task<IReadOnlyList<Gist>> GetAllPublic(ApiOptions options)
        {
            Ensure.ArgumentNotNull(options, "options");

            return ApiConnection.GetAll<Gist>(ApiUrls.PublicGists(), options);
        }

        /// <summary>
        /// Lists all public gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        /// <param name="since">Only gists updated at or after this time are returned</param>
        public Task<IReadOnlyList<Gist>> GetAllPublic(DateTimeOffset since)
        {
            return GetAllPublic(since, ApiOptions.None);
        }

        /// <summary>
        /// Lists all public gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        /// <param name="since">Only gists updated at or after this time are returned</param>
        /// <param name="options">Options for changing the API response</param>
        public Task<IReadOnlyList<Gist>> GetAllPublic(DateTimeOffset since, ApiOptions options)
        {
            Ensure.ArgumentNotNull(options, "options");

            var request = new GistRequest(since);
            return ApiConnection.GetAll<Gist>(ApiUrls.PublicGists(), request.ToParametersDictionary(), options);
        }

        /// <summary>
        /// List the authenticated user’s starred gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        public Task<IReadOnlyList<Gist>> GetAllStarred()
        {
            return GetAllStarred(ApiOptions.None);
        }

        /// <summary>
        /// List the authenticated user’s starred gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        /// <param name="options">Options for changing the API response</param>
        public Task<IReadOnlyList<Gist>> GetAllStarred(ApiOptions options)
        {
            Ensure.ArgumentNotNull(options, "options");

            return ApiConnection.GetAll<Gist>(ApiUrls.StarredGists(), options);
        }

        /// <summary>
        /// List the authenticated user’s starred gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        /// <param name="since">Only gists updated at or after this time are returned</param>
        public Task<IReadOnlyList<Gist>> GetAllStarred(DateTimeOffset since)
        {
            return GetAllStarred(since, ApiOptions.None);
        }

        /// <summary>
        /// List the authenticated user’s starred gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        /// <param name="since">Only gists updated at or after this time are returned</param>
        /// <param name="options">Options for changing the API response</param>
        public Task<IReadOnlyList<Gist>> GetAllStarred(DateTimeOffset since, ApiOptions options)
        {
            Ensure.ArgumentNotNull(options, "options");

            var request = new GistRequest(since);
            return ApiConnection.GetAll<Gist>(ApiUrls.StarredGists(), request.ToParametersDictionary(), options);
        }

        /// <summary>
        /// List a user's gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        /// <param name="user">The user</param>
        public Task<IReadOnlyList<Gist>> GetAllForUser(string user)
        {
            Ensure.ArgumentNotNullOrEmptyString(user, "user");

            return GetAllForUser(user, ApiOptions.None);
        }

        /// <summary>
        /// List a user's gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        /// <param name="user">The user</param>
        /// <param name="options">Options for changing the API response</param>
        public Task<IReadOnlyList<Gist>> GetAllForUser(string user, ApiOptions options)
        {
            Ensure.ArgumentNotNullOrEmptyString(user, "user");
            Ensure.ArgumentNotNull(options, "options");

            return ApiConnection.GetAll<Gist>(ApiUrls.UsersGists(user), options);
        }

        /// <summary>
        /// List a user's gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        /// <param name="user">The user</param>
        /// <param name="since">Only gists updated at or after this time are returned</param>
        public Task<IReadOnlyList<Gist>> GetAllForUser(string user, DateTimeOffset since)
        {
            Ensure.ArgumentNotNullOrEmptyString(user, "user");

            return GetAllForUser(user, since, ApiOptions.None);
        }

        /// <summary>
        /// List a user's gists
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists
        /// </remarks>
        /// <param name="user">The user</param>
        /// <param name="since">Only gists updated at or after this time are returned</param>
        /// <param name="options">Options for changing the API response</param>
        public Task<IReadOnlyList<Gist>> GetAllForUser(string user, DateTimeOffset since, ApiOptions options)
        {
            Ensure.ArgumentNotNullOrEmptyString(user, "user");
            Ensure.ArgumentNotNull(options, "options");

            var request = new GistRequest(since);
            return ApiConnection.GetAll<Gist>(ApiUrls.UsersGists(user), request.ToParametersDictionary(), options);
        }

        /// <summary>
        /// List gist commits
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists-commits
        /// </remarks>
        /// <param name="id">The id of the gist</param>
        public Task<IReadOnlyList<GistHistory>> GetAllCommits(string id)
        {
            Ensure.ArgumentNotNullOrEmptyString(id, "id");

            return GetAllCommits(id, ApiOptions.None);
        }

        /// <summary>
        /// List gist commits
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists-commits
        /// </remarks>
        /// <param name="id">The id of the gist</param>
        /// <param name="options">Options for changing the API response</param>
        public Task<IReadOnlyList<GistHistory>> GetAllCommits(string id, ApiOptions options)
        {
            Ensure.ArgumentNotNullOrEmptyString(id, "id");
            Ensure.ArgumentNotNull(options, "options");

            return ApiConnection.GetAll<GistHistory>(ApiUrls.GistCommits(id), options);
        }

        /// <summary>
        /// List gist forks
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists-forks
        /// </remarks>
        /// <param name="id">The id of the gist</param>
        public Task<IReadOnlyList<GistFork>> GetAllForks(string id)
        {
            Ensure.ArgumentNotNullOrEmptyString(id, "id");

            return GetAllForks(id, ApiOptions.None);
        }

        /// <summary>
        /// List gist forks
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#list-gists-forks
        /// </remarks>
        /// <param name="id">The id of the gist</param>
        /// <param name="options">Options for changing the API response</param>
        public Task<IReadOnlyList<GistFork>> GetAllForks(string id, ApiOptions options)
        {
            Ensure.ArgumentNotNullOrEmptyString(id, "id");
            Ensure.ArgumentNotNull(options, "options");

            return ApiConnection.GetAll<GistFork>(ApiUrls.ForkGist(id), options);
        }

        /// <summary>
        /// Edits a gist
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#delete-a-gist
        /// </remarks>
        /// <param name="id">The id of the gist</param>
        /// <param name="gistUpdate">The update to the gist</param>
        public Task<Gist> Edit(string id, GistUpdate gistUpdate)
        {
            Ensure.ArgumentNotNullOrEmptyString(id, "id");
            Ensure.ArgumentNotNull(gistUpdate, "gistUpdate");

            var filesAsJsonObject = new JsonObject();
            foreach (var kvp in gistUpdate.Files)
            {
                filesAsJsonObject.Add(kvp.Key, new { Content = kvp.Value.Content, Filename = kvp.Value.NewFileName });
            }

            var gist = new
            {
                Description = gistUpdate.Description,
                Files = filesAsJsonObject
            };

            return ApiConnection.Patch<Gist>(ApiUrls.Gist(id), gist);
        }

        /// <summary>
        /// Stars a gist
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#star-a-gist
        /// </remarks>
        /// <param name="id">The id of the gist</param>
        public Task Star(string id)
        {
            Ensure.ArgumentNotNullOrEmptyString(id, "id");

            return ApiConnection.Put(ApiUrls.StarGist(id));
        }

        /// <summary>
        /// Unstars a gist
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#unstar-a-gist
        /// </remarks>
        /// <param name="id">The id of the gist</param>
        public Task Unstar(string id)
        {
            Ensure.ArgumentNotNullOrEmptyString(id, "id");

            return ApiConnection.Delete(ApiUrls.StarGist(id));
        }

        /// <summary>
        /// Checks if the gist is starred
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/gists/#check-if-a-gist-is-starred
        /// </remarks>
        /// <param name="id">The id of the gist</param>
        public async Task<bool> IsStarred(string id)
        {
            Ensure.ArgumentNotNullOrEmptyString(id, "id");

            try
            {
                var response = await Connection.Get<object>(ApiUrls.StarGist(id), null, null).ConfigureAwait(false);
                return response.HttpResponse.IsTrue();
            }
            catch (NotFoundException)
            {
                return false;
            }
        }
    }
}