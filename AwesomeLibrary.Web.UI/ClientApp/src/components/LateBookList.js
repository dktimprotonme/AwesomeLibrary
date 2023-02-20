import React, { Component } from 'react';

export class LateBookList extends Component {

    constructor(props) {
        super(props);
        this.state = { lateBooks: [], loading: true };
    }

    componentDidMount() {
        this.populateLateBooks();
    }

    static renderLateBooksTable(lateBooks) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>ISBN</th>
                        <th>Book Name</th>
                        <th>Member Id</th>
                        <th>Member Full Name</th>
                        <th>Transaction Date</th>
                        <th>Expected Return Date</th>
                        <th>Late Days</th>
                        <th>Penalty</th>
                    </tr>
                </thead>
                <tbody>
                    {lateBooks.map(x =>
                        <tr>
                            <td>{x.isbn}</td>
                            <td>{x.bookName}</td>
                            <td>{x.memberId}</td>
                            <td>{x.memberFullName}</td>
                            <td>{x.transactionDate}</td>
                            <td>{x.expectedReturnDate}</td>
                            <td>{x.lateDays}</td>
                            <td>{x.penalty}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : LateBookList.renderLateBooksTable(this.state.lateBooks);

        return (
            <div>
                <h4 id="tabelLabel">Late Books</h4>
                {contents}
            </div>
        );
    }

    async populateLateBooks() {
        const response = await fetch(`${process.env.REACT_APP_API_URL}/book-transactions/late-books`);
        const data = await response.json();
        this.setState({ lateBooks: data, loading: false });
    }

}
