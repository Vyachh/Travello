import { ITrip } from './trip';
export interface IUserInfo {
    id: string
    userName: string
    currentTripId: number
    tripList: ITrip[]
    role: string
}