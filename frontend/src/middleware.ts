import { NextResponse } from "next/server"
import type { NextRequest } from "next/server"
import { decodeJwt } from "./utils/helpers/jwt"

export function middleware(req: NextRequest) {
    const refreshToken = req.cookies.get("refresh_token")?.value

    if (!refreshToken) {
        return NextResponse.redirect(new URL("/?reason=no_token", req.url))
    }

    const payload = decodeJwt(refreshToken)

    if (!payload || !payload.exp || Date.now() >= payload.exp * 1000) {
        return NextResponse.redirect(new URL("/?reason=expired", req.url))
    }

    console.log("hehe")
    return NextResponse.next()
}

export const config = {
    matcher: ["/profile/:path*"]
}
