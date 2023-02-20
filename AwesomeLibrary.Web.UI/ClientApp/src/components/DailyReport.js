import React, { Component } from 'react';
import { UpcomingBookList } from './UpcomingBookList';
import { LateBookList } from './LateBookList';

export class DailyReport extends Component {
    render() {
        return (
            <div>
                <h1>Daily Report</h1>
                <hr/>
                <UpcomingBookList />
                <br />
                <LateBookList />
            </div>
        );
    }
}
