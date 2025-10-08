import React from "react"
import { ButtonStyled } from "../ButtonStyled"
export function ButtonSendStyled() {
    return (
        <ButtonStyled
            className="flex items-center bg-primary text-white gap-1 px-24 py-2
         cursor-pointer font-semibold tracking-widest rounded-md hover:bg-primary hover:opacity-75 duration-300 hover:gap-2 hover:translate-x-3"
        >
            Send
            <svg
                className="w-5 h-5"
                stroke="currentColor"
                strokeWidth="1.5"
                viewBox="0 0 24 24"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
            >
                <path
                    d="M6 12 3.269 3.125A59.769 59.769 0 0 1 21.485 12 59.768 59.768 0 0 1 3.27 20.875L5.999 12Zm0 0h7.5"
                    strokeLinejoin="round"
                    strokeLinecap="round"
                />
            </svg>
        </ButtonStyled>
    )
}
