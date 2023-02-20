import React, { Component } from 'react';

export class BookTransactionList extends Component {

    constructor(props) {
        super(props);
        this.state = { bookTransactions: [], loading: true };
    }

    componentDidMount() {
        this.populateBookTransactions();
    }

    static renderBookTransactionsTable(bookTransactions) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>ISBN</th>
                        <th>Member Id</th>
                        <th>Transaction Date</th>
                        <th>Expected Return Date</th>
                        <th>Real Return Date</th>
                    </tr>
                </thead>
                <tbody>
                    {bookTransactions.map(x =>
                        <tr>
                            <td>{x.id}</td>
                            <td>{x.isbn}</td>
                            <td>{x.memberId}</td>
                            <td>{x.transactionDate}</td>
                            <td>{x.expectedReturnDate}</td>
                            <td>{x.realReturnDate}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : BookTransactionList.renderBookTransactionsTable(this.state.bookTransactions);

        return (
            <div>
                <h4 id="tabelLabel">Book Transactions</h4>
                {contents}
            </div>
        );
    }

    async populateBookTransactions() {
        const response = await fetch(`${process.env.REACT_APP_API_URL}/book-transactions`);
        const data = await response.json();
        this.setState({ bookTransactions: data, loading: false });
    }
}
