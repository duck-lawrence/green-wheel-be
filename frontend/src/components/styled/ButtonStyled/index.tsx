"use client"
import React from "react"
import { Button, ButtonProps, cn, Spinner } from "@heroui/react"

export function ButtonStyled(props: ButtonProps) {
    return (
        <>
            {props.isLoading ? (
                <Spinner />
            ) : (
                <Button
                    color="secondary"
                    {...props}
                    className={cn("font-medium text-base", props.className)}
                />
            )}
        </>
    )
}
