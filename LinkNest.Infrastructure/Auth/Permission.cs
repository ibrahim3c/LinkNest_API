namespace LinkNest.Infrastructure.Auth
{
    public enum Permission
    {
        //Account
        Account_Read=1,
        Account_LockUnlock,
        Account_ResetPassword,
        Account_ForgotPassword,
        Auth_Login,
        Auth_Register,
        Auth_RefreshToken,
        Auth_RevokeToken,


        // User Profile Domain
        UserProfile_Read,
        UserProfile_ReadAll,
        UserProfile_Update,

        // Post Domain
        Post_Read,
        Post_ReadAll,
        Post_Create,
        Post_Update,
        Post_Delete,

        // Post Comment Subdomain
        PostComment_Create,
        PostComment_Read,
        PostComment_Update,
        PostComment_Delete,

        // Post Interaction Subdomain
        PostInteraction_Create,
        PostInteraction_Read,
        PostInteraction_Delete,

        // Social/Follow Domain
        Follow_Manage,
        Follow_ReadFollowers,
        Follow_ReadFollowees,

        // Role Management
        Role_Manage
    }
}