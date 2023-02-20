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

export class ReturnBook extends Component {

    constructor(props) {
        super(props);
        this.state = { memberId: null, bookSearchResultList: [], selectedISBNs: [] };
    }

    render() {
        return (
            <div>
                <h1>Return Book</h1>
                <hr />
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
                <br/>
                <Button id="bookSearch" color="primary" onClick={this.bookSearch}>
                    <BiSearch /> Book Search
                </Button>
                <br />
                <br />
                <Row>
                    <Col>
                        <div>
                            <h4 id="tabelLabel">Book Search Result</h4>
                            <table className='table table-striped' aria-labelledby="tabelLabel">
                                <thead>
                                    <tr>
                                        <th>Select</th>
                                        <th>ISBN</th>
                                        <th>Book Name</th>
                                        <th>Member Full Name</th>
                                        <th>Transaction Date</th>
                                        <th>Expected Return Date</th>
                                        <th>Late Days</th>
                                        <th>Penalty</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {this.state.bookSearchResultList.map(x =>
                                        <tr>
                                            <td><Input type="checkbox" value={x.isbn} onChange={this.selectBook} /></td>
                                            <td>{x.isbn}</td>
                                            <td>{x.bookName}</td>
                                            <td>{x.memberFullName}</td>
                                            <td>{x.transactionDate}</td>
                                            <td>{x.expectedReturnDate}</td>
                                            <td>{x.hasPenalty ? <Badge color="danger">{x.lateDays}</Badge> : ""}</td>
                                            <td>{x.hasPenalty ? <Badge color="danger">{x.penalty}</Badge> : ""}</td>
                                        </tr>
                                    )}
                                </tbody>
                            </table>
                        </div>
                    </Col>
                </Row>
                <br />
                <Button id="bookReturn" color="primary" onClick={this.bookReturn}>
                    <BiBook /> Return Book
                </Button>
                <br />
            </div>
        );
    }

    bookSearch = async () => {

        if (this.state.memberId == null) {
            Report.info("AwesomeLibrary Info", "Enter Member Id", "OK");
            return;
        }

        const request = {
            memberId: this.state.memberId,
        };

        fetch(`${process.env.REACT_APP_API_URL}/books/return-search`, {
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

    bookReturn = async () => {

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

        fetch(`${process.env.REACT_APP_API_URL}/book-transactions/return`, {
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
                const hasPenalty = data.hasPenalty;
                const totalPenalty = data.totalPenalty;
                let message = "Books was returned successfully.";
                if (hasPenalty) {
                    message = message + " <b>Total Penalty = " + totalPenalty + "</b>";
                }
                Report.success("AwesomeLibrary Success", message, "OK", () => window.location.reload());
            })
            .catch((error) => {
                console.error('Error:', error);
            });
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