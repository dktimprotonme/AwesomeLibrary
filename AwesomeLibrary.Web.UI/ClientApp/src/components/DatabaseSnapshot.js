import React, { Component } from 'react';
import { MemberList } from './MemberList';
import { BookList } from './BookList';
import { BookTransactionList } from './BookTransactionList';

export class DatabaseSnapshot extends Component {
    render() {
        return (
            <div>
                <h1>Database Snapshot</h1>
                <p><b>Note: </b>This page shows all table values in database to test other functions easily.</p>
                <hr />
                <MemberList />
                <br />
                <BookList />
                <br />
                <BookTransactionList />
            </div>
        );
    }
}
