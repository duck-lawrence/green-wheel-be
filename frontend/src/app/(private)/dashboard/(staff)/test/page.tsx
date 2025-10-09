"use client"

import React, { useMemo, useState } from "react"
import { useTranslation } from "react-i18next"
import { PaginationStyled } from "@/components"
/** ====== Giả lập kiểu dữ liệu backend ====== */
type StaffItem = { id: number; name: string; email: string }

type PagedResult<T> = {
  items: T[]              // Items
  pageNumber: number      // PageNumber (1-based)
  pageSize: number        // PageSize
  totalCount: number      // TotalCount
  totalPage: number       // TotalPage = ceil(totalCount / pageSize)
}

/** ====== HARD-CODE DATA ====== */
const TOTAL_COUNT = 101              // TotalCount (cứng)
const DEFAULT_PAGE_SIZE = 10          // PageSize (cứng 10 như yêu cầu)
const ALL_ITEMS: StaffItem[] = Array.from({ length: TOTAL_COUNT }, (_, i) => ({
  id: i + 1,
  name: `User ${i + 1}`,
  email: `user${i + 1}@example.com`,
}))

/** Hàm giả lập backend trả về PagedResult<T> */
function getPagedStaff(pageNumber: number, pageSize: number): PagedResult<StaffItem> {
  const totalCount = TOTAL_COUNT
  const totalPage = Math.max(1, Math.ceil(totalCount / Math.max(1, pageSize)))
  const safePage = Math.min(Math.max(1, pageNumber), totalPage)
  const start = (safePage - 1) * pageSize
  const end = start + pageSize

  return {
    items: ALL_ITEMS.slice(start, end),
    pageNumber: safePage,
    pageSize,
    totalCount,
    totalPage,
  }
}

/** ====== PAGE DEMO (hardcode) ====== */
export default function StaffTestPage() {
  const [page, setPage] = useState(1)
  const pageSize = DEFAULT_PAGE_SIZE

  // “Gọi backend” nhưng ở đây là tính cứng trong memory
  const data = useMemo(() => getPagedStaff(page, pageSize), [page, pageSize])

  return (
    <div className="rounded-2xl bg-white px-6 py-8 shadow-sm space-y-6">
      <header>
        <h1 className="text-2xl font-semibold text-gray-900">Test Page (Hardcoded)</h1>

      </header>

      {/* Bảng xem trước Items hiện tại */}
      <section className="space-y-3">
        <div className="rounded-xl border border-dashed border-slate-200 p-4 text-sm text-slate-700">
          <div>Page: <b>{data.pageNumber}</b> / <b>{data.totalPage}</b></div>
          <div>PageSize: <b>{data.pageSize}</b></div>
          <div>TotalCount: <b>{data.totalCount}</b></div>
        </div>

        <div className="rounded-xl border border-slate-200 overflow-hidden">
          <table className="w-full text-left text-sm">
            <thead className="bg-slate-50 text-slate-600">
              <tr>
                <th className="px-4 py-2">ID</th>
                <th className="px-4 py-2">Name</th>
                <th className="px-4 py-2">Email</th>
              </tr>
            </thead>
            <tbody>
              {data.items.map((it) => (
                <tr key={it.id} className="border-t">
                  <td className="px-4 py-2">{it.id}</td>
                  <td className="px-4 py-2">{it.name}</td>
                  <td className="px-4 py-2">{it.email}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* Phân trang dùng PaginationStyled */}
        <div className="pt-2">
          <PaginationStyled
            pageNumber={data.pageNumber}        // PageNumber từ “backend”
            pageSize={data.pageSize}            // PageSize từ “backend”
            totalItems={data.totalCount}        // TotalCount từ “backend”
            onPageChange={setPage}              // đổi trang -> tính lại data
            showControls
            // // Dùng màu hệ thống (có thể đổi):
            // activeBgClass="bg-primary"
            // activeTextClass="text-white"
            // controlHoverClassName="hover:bg-primary hover:text-white"
            // // Không set prevContent/nextContent -> mặc định hiển thị "<" / ">"
          />
        </div>
      </section>
    </div>
  )
}