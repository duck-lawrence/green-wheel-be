import React from "react"

export default function LoadingStyled() {
    return (
        // <div className="flex items-center justify-center min-h-screen">
        //     <div className="relative">
        //         <div className="relative w-32 h-32">
        //             <div
        //                 className="absolute w-full h-full rounded-full border-[3px] border-gray-100/10 border-r-primary border-b-primary animate-spin"
        //                 style={{ animationDuration: "3s" }}
        //             />
        //             <div
        //                 className="absolute w-full h-full rounded-full border-[3px] border-gray-100/10 border-t-primary animate-spin"
        //                 style={{ animationDuration: "2s", animationDirection: "reverse" }}
        //             />
        //         </div>
        //         <div className="absolute inset-0 bg-gradient-to-tr from-[#0ff]/10 via-transparent to-[#0ff]/5 animate-pulse rounded-full blur-sm" />
        //     </div>
        // </div>
        <div className="w-32 aspect-square rounded-full relative flex justify-center items-center animate-[spin_3s_linear_infinite] z-40 bg-[conic-gradient(white_0deg,white_300deg,transparent_270deg,transparent_360deg)] before:animate-[spin_2s_linear_infinite] before:absolute before:w-[60%] before:aspect-square before:rounded-full before:z-[80] before:bg-[conic-gradient(white_0deg,white_270deg,transparent_180deg,transparent_360deg)] after:absolute after:w-3/4 after:aspect-square after:rounded-full after:z-[60] after:animate-[spin_3s_linear_infinite] after:bg-[conic-gradient(#065f46_0deg,#065f46_180deg,transparent_180deg,transparent_360deg)]">
            <span className="absolute w-[85%] aspect-square rounded-full z-[60] animate-[spin_5s_linear_infinite] bg-[conic-gradient(#34d399_0deg,#34d399_180deg,transparent_180deg,transparent_360deg)]"></span>
        </div>
    )
}
