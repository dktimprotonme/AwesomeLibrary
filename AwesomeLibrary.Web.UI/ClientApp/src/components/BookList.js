import React, { Component } from 'react';

export class BookList extends Component {

    constructor(props) {
        super(props);
        this.state = { books: [], loading: true };
    }

    componentDidMount() {
        this.populateBooks();
    }

    static renderBooksTable(books) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>ISBN</th>
                        <th>Name</th>
                        <th>Author</th>
                    </tr>
                </thead>
                <tbody>
                    {books.map(x =>
                        <tr>
                            <td>{x.isbn}</td>
                            <td>{x.name}</td>
                            <td>{x.author}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : BookList.renderBooksTable(this.state.books);

        return (
            <div>
                <h4 id="tabelLabel">Books</h4>
                {contents}
            </div>
        );
    }

    async populateBooks() {
        const response = await fetch(`${process.env.REACT_APP_API_URL}/books`);
        const data = await response.json();
        this.setState({ books: data, loading: false });
    }

}
