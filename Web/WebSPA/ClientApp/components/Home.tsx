import * as Oidc from 'oidc-client'
import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { withRouter } from 'react-router-dom'


export default class Home extends React.Component<RouteComponentProps<{}>, {}> {

    public render() {

        const LoginButton = withRouter(({ history }) => (
            <button
              type='button'
              className='btn btn-default'
              onClick={() => { history.push('/oidc-handler') }}
            >
              Login
            </button>
          ))

        return <div>
            <LoginButton />            
        </div>;
    }
}
