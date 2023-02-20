import React, { Component } from 'react';

export class UpcomingBookList extends Component {

    constructor(props) {
        super(props);
        this.state = { upcomingBooks: [], loading: true };
    }

    componentDidMount() {
        this.populateUpcomingBooks();
    }

    static renderUpcomingBooksTable(upcomingBooks) {
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
                        <th>Upcoming Days</th>
                    </tr>
                </thead>
                <tbody>
                    {upcomingBooks.map(x =>
                        <tr>
                            <td>{x.isbn}</td>
                            <td>{x.bookName}</td>
                            <td>{x.memberId}</td>
                            <td>{x.memberFullName}</td>
                            <td>{x.transactionDate}</td>
                            <td>{x.expectedReturnDate}</td>
                            <td>{x.upcomingDays}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : UpcomingBookList.renderUpcomingBooksTable(this.state.upcomingBooks);

        return (
            <div>
                <h4 id="tabelLabel">Upcoming Books</h4>
                {contents}
            </div>
        );
    }

    async populateUpcomingBooks() {
        const response = await fetch(`${process.env.REACT_APP_API_URL}/book-transactions/upcoming-books`);
        const data = await response.json();
        this.setState({ upcomingBooks: data, loading: false });
    }

}
