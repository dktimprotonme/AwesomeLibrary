import React, { Component } from "react";
import {
    Input,
    Row,
    Col,
    Button,
    Label,
    Badge
} from "reactstrap";
import { BiSearch, BiBook } from "react-icons/bi";
import { Report } from 'notiflix/build/notiflix-report-aio';

export class BorrowBook extends Component {

    constructor(props) {
        super(props);
        this.state = { memberId: null, isbn: "", bookName: "", bookAuthor: "", bookSearchResultList: [], selectedISBNs: [] };
    }

    render() {
        return (
            <div>
                <h1>Borrow Book</h1>
                <hr />
                <Row>
                    <Col md={3}>
                        <Label for="isbn">
                            <b>ISBN</b>
                        </Label>
                        <Input
                            id="isbn"
                            name="isbn"
                            type="text"
                            value={this.state.isbn}
                            onChange={this.onInputChange}
                        />
                    </Col>
                    <Col md={6}>
                        <Label for="bookName">
                            <b>Book Name</b>
                        </Label>
                        <Input
                            id="bookName"
                            name="bookName"
                            type="text"
                            value={this.state.bookName}
                            onChange={this.onInputChange}
                        />
                    </Col>
                    <Col md={3}>
                        <Label for="bookAuthor">
                            <b>Book Author</b>
                        </Label>
                        <Input
                            id="bookAuthor"
                            name="bookAuthor"
                            type="text"
                            value={this.state.bookAuthor}
                            onChange={this.onInputChange}
                        />
                    </Col>
                </Row>
                <br />
                <Button id="bookSearch" color="primary" onClick={this.bookSearch}>
                    <BiSearch /> Book Search
                </Button>
                <br />
                <br />
                <div>
                    <h4 id="tabelLabel">Book Search Result</h4>
                    <table className='table table-striped' aria-labelledby="tabelLabel">
                        <thead>
                            <tr>
                                <th>Select</th>
                                <th>ISBN</th>
                                <th>Name</th>
                                <th>Author</th>
                                <th>Status</th>
                                <th>Expected Return Date</th>
                            </tr>
                        </thead>
                        <tbody>
                            {this.state.bookSearchResultList.map(x =>
                                <tr>
                                    <td>{x.isAvailable ? <Input type="checkbox" value={x.isbn} onChange={this.selectBook} /> : ""}</td>
                                    <td>{x.isbn}</td>
                                    <td>{x.bookName}</td>
                                    <td>{x.bookAuthor}</td>
                                    <td>{x.isAvailable ? <Badge color="success">Available</Badge> : <Badge color="danger">Not Available</Badge>}</td>
                                    <td>{x.expectedReturnDate}</td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </div>
                <Row>
                    <Col md={3}>
                        <Label for="memberId">
                            <b>Member Id</b>
                        </Label>
                        <Input
                            id="memberId"
                            name="memberId"
                            type="number"
                            value={this.state.memberId}
                            onChange={this.onInputChange}
                        />
                    </Col>
                </Row>
                <br />
                <Button id="bookBorrow" color="primary" onClick={this.bookBorrow}>
                    <BiBook /> Borrow Book
                </Button>
            </div>
        );
    }

    bookBorrow = async () => {

        if (this.state.memberId == null) {
            Report.info("AwesomeLibrary Info", "Enter Member Id", "OK");
            return;
        }

        if (this.state.selectedISBNs.length == 0) {
            Report.info("AwesomeLibrary Info", "Select a Book", "OK");
            return;
        }

        const request = {
            memberId: this.state.memberId,
            selectedISBNs: this.state.selectedISBNs
        };

        fetch(`${process.env.REACT_APP_API_URL}/book-transactions/borrow`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(request),
        })
            .then((response) => response.json())
            .then((data) => {
                if (data.isError) {
                    Report.failure("AwesomeLibrary Failure", data.errorMessage, "OK");
                    return;
                }
                Report.success("AwesomeLibrary Success", "Books was borrowed successfully. Expected return date = " + data.expectedReturnDate, "OK", () => window.location.reload());
            })
            .catch((error) => {
                console.error('Error:', error);
            });
    };

    bookSearch = async () => {

        if (this.state.isbn == "" && this.state.bookName == "" && this.state.bookAuthor == "") {
            Report.info("AwesomeLibrary Info", "Enter ISBN or Book Name or Book Author", "OK");
            return;
        }

        const request = {
            isbn: this.state.isbn,
            bookName: this.state.bookName,
            bookAuthor: this.state.bookAuthor
        };

        fetch(`${process.env.REACT_APP_API_URL}/books/borrow-search`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(request),
        })
            .then((response) => response.json())
            .then((data) => {
                if (data.isError) {
                    Report.failure("AwesomeLibrary Failure", data.errorMessage, "OK");
                    return;
                }
                this.setState({ bookSearchResultList: data });
            })
            .catch((error) => {
                console.error('Error:', error);
            });;
    };

    onInputChange = (event) => {
        this.setState({
            [event.target.name]: event.target.value
        });
    };

    selectBook = (event) => {
        const checked = event.target.checked;
        const isbn = event.target.value;
        if (checked) {
            const list = this.state.selectedISBNs;
            list.push(isbn);
            this.setState({ selectedISBNs: list });
        }
        else {
            const list = this.state.selectedISBNs.filter(item => item !== isbn)
            this.setState({ selectedISBNs: list });
        }
    };
}