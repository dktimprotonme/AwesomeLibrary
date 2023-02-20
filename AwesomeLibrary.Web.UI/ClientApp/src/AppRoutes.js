import { DatabaseSnapshot } from "./components/DatabaseSnapshot";
import { DailyReport } from "./components/DailyReport";
import { BorrowBook } from "./components/BorrowBook";
import { ReturnBook } from "./components/ReturnBook";

const AppRoutes = [
    {
        index: true,
        element: <DatabaseSnapshot />
    },
    {
        path: '/database-snapshot',
        element: <DatabaseSnapshot />
    },
    {
        path: '/borrow-book',
        element: <BorrowBook />
    },
    {
        path: '/return-book',
        element: <ReturnBook />
    },
    {
        path: '/daily-report',
        element: <DailyReport />
    }
];

export default AppRoutes;
