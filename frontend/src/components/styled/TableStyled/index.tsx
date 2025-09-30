"use client"
import { Table, TableHeader, TableColumn, TableBody, TableRow, TableCell } from "@heroui/react"
import React from "react"
// { data, loading }

export default function TableStyled({ data, loading }) {
    if (loading) return <div className="text-center">⏳ Loading order...</div>
    if (!data || data.length === 0) return <div className="text-center">No order</div>
    return (
        <Table aria-label="Example static collection table" className="w-full">
            <TableHeader>
                <TableColumn className="text-xl text-center">Order</TableColumn>
                <TableColumn className="text-xl text-center">Vehicle model</TableColumn>
                <TableColumn className="text-xl text-center">Pickup time</TableColumn>
                <TableColumn className="text-xl text-center">Return time</TableColumn>
                <TableColumn className="text-xl text-center">Pickup address</TableColumn>
                <TableColumn className="text-xl text-center">Status</TableColumn>
            </TableHeader>

            <TableBody>
                {data.map((item) => (
                    <TableRow key={item.order} className="border-b border-gray-300">
                        <TableCell className="text-center">{item.order}</TableCell>
                        <TableCell className="text-center">{item.model}</TableCell>
                        <TableCell className="text-center">{item.pickupTime}</TableCell>
                        <TableCell className="text-center">{item.returnTime}</TableCell>
                        <TableCell className="text-center">{item.pickupAddress}</TableCell>
                        <TableCell className="text-center">{item.status}</TableCell>
                    </TableRow>
                ))}
            </TableBody>
        </Table>
    )
}
{
    /* <TableCell>
                        <div className="flex justify-center items-center h-full w-full">
                            Tony Reichert
                        </div>
                    </TableCell>
                    <TableCell>
                        <div className="flex justify-center items-center h-full w-full">CEO</div>
                    </TableCell>
                    <TableCell>
                        <div className="flex justify-center items-center h-full w-full">Active</div>
                    </TableCell>
                    <TableCell>
                        <div className="flex justify-center items-center h-full w-full">Active</div>
                    </TableCell>
                    <TableCell>
                        <div className="flex justify-center items-center h-full w-full">Active</div>
                    </TableCell>
                    <TableCell>
                        <div className="flex justify-center items-center h-full w-full">Active</div>
                    </TableCell> */
}
