import * as Oidc from 'oidc-client'
import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import accountManager from './AccountManager'
import { Redirect } from 'react-router-dom'

export default class OidcHandler extends React.Component<RouteComponentProps<{}>, {}> {

    public render() {
        if (window.location.hash.indexOf("access_token") > -1) {
            accountManager.signinRedirectCallback();
            return <Redirect to="/" />
        }
        else {
            accountManager.signinRedirect();
        }
        return <div></div>
    }

}