import React, { Component } from 'react';

export class MemberList extends Component {

    constructor(props) {
        super(props);
        this.state = { members: [], loading: true };
    }

    componentDidMount() {
        this.populateMembers();
    }

    static renderMembersTable(members) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                        <th>Surname</th>
                    </tr>
                </thead>
                <tbody>
                    {members.map(x =>
                        <tr>
                            <td>{x.id}</td>
                            <td>{x.name}</td>
                            <td>{x.surname}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : MemberList.renderMembersTable(this.state.members);

        return (
            <div>
                <h4 id="tabelLabel">Members</h4>
                {contents}
            </div>
        );
    }

    async populateMembers() {
        const response = await fetch(`${process.env.REACT_APP_API_URL}/members`);
        const data = await response.json();
        this.setState({ members: data, loading: false });
    }
}
