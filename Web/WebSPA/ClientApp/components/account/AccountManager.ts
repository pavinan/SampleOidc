import * as Oidc from 'oidc-client'

let userManager = new Oidc.UserManager({
    authority: "http://localhost:5105",
    client_id: 'js',
    redirect_uri: 'http://localhost:5104/oidc-handler',
    response_type: 'id_token token',
    scope: 'openid profile contacts'
});



export default userManager;