import { DEFAULT_DATE_TIME_FORMAT, DEFAULT_TIMEZONE } from "@/constants/constants"
import dayjs from "dayjs"
import { DateValue, parseDateTime } from "@internationalized/date"

export const useDay = ({
    defaultFormat = DEFAULT_DATE_TIME_FORMAT
}: {
    defaultFormat?: string
} = {}) => {
    const toCalenderDateTime = (
        dateTime: string | number | Date | dayjs.Dayjs | null | undefined
    ) => {
        if (!dateTime) return null
        const str = dayjs(dateTime).format(defaultFormat)
        return parseDateTime(str)
    }

    const formatDateTime = ({
        date,
        timeZone = DEFAULT_TIMEZONE
    }: {
        date: DateValue | string
        timeZone?: string
    }) => {
        if (!date) return ""

        if (typeof date === "string") {
            return dayjs(date).format(defaultFormat)
        }

        const dateJs = dayjs(date.toDate(timeZone))
        return dateJs.format(defaultFormat)
    }

    const getDiffDaysCeil = ({
        startDate,
        endDate
    }: {
        startDate: string | Date | dayjs.Dayjs | null
        endDate: string | Date | dayjs.Dayjs | null
    }) => {
        if (!startDate || !endDate) return -1
        return Math.ceil(dayjs(endDate).diff(dayjs(startDate), "day", true))
    }

    return { toCalenderDateTime, formatDateTime, getDiffDaysCeil }
}
