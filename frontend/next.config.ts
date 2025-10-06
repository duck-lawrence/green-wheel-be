import type { NextConfig } from "next"

const nextConfig: NextConfig = {
    /* config options here */
    images: {
        remotePatterns: [
            {
                protocol: "https",
                hostname: "vinfastninhbinh.com.vn",
                pathname: "/**"
            },
            {
                protocol: "https",
                hostname: "vinfasttimescity.vn",
                pathname: "/**"
            },
            {
                protocol: "https",
                hostname: "vinfast-cars.vn",
                pathname: "/**"
            },
            {
                protocol: "https",
                hostname: "vinfast-khanhhoa.com",
                pathname: "/**"
            },
            {
                protocol: "https",
                hostname: "vinfastnewway.com.vn",
                pathname: "/**"
            },
            {
                protocol: "https",
                hostname: "vinfastvinhphuc.vn",
                pathname: "/**"
            },
            {
                protocol: "https",
                hostname: "marketplace.canva.com",
                pathname: "/**"
            },
            {
                protocol: "https",
                hostname: "img.pikbest.com",
                pathname: "/**"
            }
        ]
    }
}

export default nextConfig
