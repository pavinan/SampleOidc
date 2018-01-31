import * as React from 'react';
import { NavMenu } from './NavMenu';
import { TopNavMenu } from './TopNavMenu'

export class Layout extends React.Component<{}, {}> {
    public render() {
        return <div className='container-fluid'>
            <div className='row'>
                <div className='col-sm-3'>
                    <NavMenu />
                </div>
                <div className='col-sm-9'>
                    <TopNavMenu />
                    {this.props.children}
                </div>
            </div>
        </div>;
    }
}
