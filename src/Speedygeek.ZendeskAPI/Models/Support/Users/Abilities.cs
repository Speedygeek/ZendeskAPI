// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Speedygeek.ZendeskAPI.Models.Support
{
    /// <summary>
    /// Side-load object for users
    /// </summary>
    public class Abilities
    {
        /// <summary>
        /// User URL
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// User Id
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Can edit
        /// </summary>
        public bool CanEdit { get; }

        /// <summary>
        /// Can Edit Password
        /// </summary>
        public bool CanEditPassword { get; }

        /// <summary>
        /// Can manage identities
        /// </summary>
        public bool CanManageIdentitiesOf { get; }

        /// <summary>
        ///  Can verify identities
        /// </summary>
        public bool CanVerifyIdentities { get; }

        /// <summary>
        /// Can reset password
        /// </summary>
        public bool CanResetPassword { get; }

        /// <summary>
        /// Can set password
        /// </summary>
        public bool CanSetPassword { get; }

        /// <summary>
        /// Can create password
        /// </summary>
        public bool CanCreatePassword { get; }

        /// <summary>
        /// Can change password
        /// </summary>
        public bool CanChangePassword { get; }

        /// <summary>
        /// Can set alias
        /// </summary>
        public bool CanSetAlias { get; }

        /// <summary>
        /// Can send verification email
        /// </summary>
        public bool CanSendVerificationEmail { get; }

        /// <summary>
        /// Can verify now
        /// </summary>
        public bool CanVerifyNow { get; }

        /// <summary>
        /// Can make comment private
        /// </summary>
        public bool CanMakeCommentPrivate { get; }

        /// <summary>
        /// Can edit agent forwarding
        /// </summary>
        public bool CanEditAgentForwarding { get; }

        /// <summary>
        /// Can modify user tags
        /// </summary>
        public bool CanModifyUserTags { get; }

        /// <summary>
        /// Can assume
        /// </summary>
        public bool CanAssume { get; }

        /// <summary>
        /// Can delete
        /// </summary>
        public bool CanDelete { get; }

        /// <summary>
        /// Can View Views
        /// </summary>
        public bool CanViewViews { get; }

        /// <summary>
        /// Can view reports
        /// </summary>
        public bool CanViewReports { get; }

        /// <summary>
        /// Can export
        /// </summary>
        public bool CanExport { get; }

        /// <summary>
        /// Can use voice console
        /// </summary>
        public bool CanUseVoiceConsole { get; }

        /// <summary>
        /// Voice enabled account
        /// </summary>
        public bool VoiceEnabledAccount { get; }

        /// <summary>
        /// Can use voice
        /// </summary>
        public bool CanUseVoice { get; }

        /// <summary>
        /// Can view voice dashboard
        /// </summary>
        public bool CanViewVoiceDashboard { get; }
    }
}
